// Services/HoustonService.cs
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using HelpDesk.Api.Dtos; // verifique namespace real do seu TriageResultDto
using Microsoft.Extensions.Configuration;

namespace HelpDesk.Api.Services
{
    public class HoustonService
    {
        private readonly IHttpClientFactory _httpFactory;
        private readonly IConfiguration _configuration;
        private readonly string _apiKey;
        private readonly string _endpoint;
        private readonly string _provider; // "openai" ou "azure"
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public HoustonService(IHttpClientFactory httpFactory, IConfiguration configuration)
        {
            _httpFactory = httpFactory ?? throw new ArgumentNullException(nameof(httpFactory));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            _apiKey = _configuration["Houston:ApiKey"] ?? string.Empty;
            _endpoint = _configuration["Houston:Endpoint"] ?? "https://api.openai.com/v1/chat/completions";
            _provider = _configuration["Houston:Provider"]?.ToLower() ?? "openai";

            // Note: we do not throw if apiKey is empty here so the service can still be used in test/dev
            // but TriageTicketAsync will fallback if the key/endpoint are invalid.
        }

        /// <summary>
        /// Principal método usado pelo TicketsController para triagem.
        /// </summary>
        public async Task<TriageResultDto> TriageTicketAsync(string titulo, string descricao)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(titulo) && string.IsNullOrWhiteSpace(descricao))
                {
                    return FallbackTriagem("Título e descrição vazios.");
                }

                if (string.IsNullOrEmpty(_apiKey) || string.IsNullOrEmpty(_endpoint))
                {
                    return FallbackTriagem("Configuração da API de IA não encontrada.");
                }

                var client = _httpFactory.CreateClient("houston");
                client.Timeout = TimeSpan.FromSeconds(25);

                // Header Authorization
                if (_provider == "azure")
                {
                    // Azure OpenAI expects "api-key" header (or Authorization depending on setup)
                    client.DefaultRequestHeaders.Remove("api-key");
                    client.DefaultRequestHeaders.Add("api-key", _apiKey);
                }
                else
                {
                    client.DefaultRequestHeaders.Remove("Authorization");
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Bearer {_apiKey}");
                }

                var prompt = BuildPrompt(titulo, descricao);

                // Request body for OpenAI Chat Completion style (works for openai.com)
                // For Azure it depends on model deployment and endpoint; storing generic body that often works.
                var requestBody = new
                {
                    model = _configuration["Houston:Model"] ?? "gpt-4o-mini",
                    messages = new[]
                    {
                        new { role = "system", content = "Você é Houston, assistente técnico especializado em triagem de chamados. Responda APENAS com JSON válido no formato solicitado." },
                        new { role = "user", content = prompt }
                    },
                    temperature = 0.2,
                    max_tokens = 600
                };

                var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

                using var response = await client.PostAsync(_endpoint, content);

                if (!response.IsSuccessStatusCode)
                {
                    // Tenta ler mensagem de erro para logs/debug
                    var body = await SafeReadStringAsync(response);
                    return FallbackTriagem($"IA retornou erro HTTP {response.StatusCode}: {Truncate(body, 400)}");
                }

                var responseJson = await response.Content.ReadAsStringAsync();

                // Tenta extrair o "message content" do response (OpenAI-style)
                string aiContent = ExtractContentFromOpenAiResponse(responseJson);

                if (string.IsNullOrWhiteSpace(aiContent))
                {
                    return FallbackTriagem("Resposta vazia da IA.");
                }

                try
                {
                    // O Houston retorna JSON conforme TriageResultDto
                    var triage = JsonSerializer.Deserialize<TriageResultDto>(aiContent, _jsonOptions);
                    if (triage == null)
                        return FallbackTriagem("Falha ao desserializar JSON da IA.");

                    // Normaliza campos mínimos
                    triage.Prioridade = string.IsNullOrWhiteSpace(triage.Prioridade) ? "Média" : triage.Prioridade;
                    triage.SetorRecomendado = string.IsNullOrWhiteSpace(triage.SetorRecomendado) ? "Suporte" : triage.SetorRecomendado;

                    return triage;
                }
                catch (JsonException)
                {
                    // Se IA retornar texto não-JSON, fallback seguro
                    return FallbackTriagem("IA retornou texto não-JSON.");
                }
            }
            catch (Exception ex)
            {
                // Se der qualquer erro (timeout, parse, etc) -> fallback
                return FallbackTriagem("Erro interno do serviço de IA: " + ex.Message);
            }
        }

        private string BuildPrompt(string titulo, string descricao)
        {
            return $@"
Classifique o chamado abaixo e RETORNE APENAS JSON VÁLIDO com as chaves:
    Prioridade, SetorRecomendado, ResumoTriagem, SolucaoSugerida

Formato esperado (exato):
{{
  ""Prioridade"": ""Baixa | Média | Alta | Crítica"",
  ""SetorRecomendado"": ""Infraestrutura | Suporte | Sistemas | Redes | Hardware | Financeiro | RH | Outros"",
  ""ResumoTriagem"": ""Texto conciso do que está acontecendo (máx 300 chars)"",
  ""SolucaoSugerida"": ""Instruções curtas para o usuário tentar antes do atendimento humano""
}}

Título: {titulo}
Descrição: {descricao}
";
        }

        private TriageResultDto FallbackTriagem(string motivo)
        {
            return new TriageResultDto
            {
                Prioridade = "Média",
                SetorRecomendado = "Suporte",
                ResumoTriagem = $"Triagem automática indisponível. Motivo: {motivo}",
                SolucaoSugerida = "Aguarde um analista avaliar o seu chamado."
            };
        }

        /// <summary>
        /// Extrai o conteúdo da resposta no formato OpenAI (choices[0].message.content).
        /// Se o JSON for diferente, tenta usar o próprio body como retorno.
        /// </summary>
        private string ExtractContentFromOpenAiResponse(string json)
        {
            try
            {
                using var doc = JsonDocument.Parse(json);
                if (doc.RootElement.TryGetProperty("choices", out var choices) && choices.GetArrayLength() > 0)
                {
                    var first = choices[0];
                    if (first.TryGetProperty("message", out var message) && message.TryGetProperty("content", out var content))
                    {
                        return content.GetString() ?? string.Empty;
                    }

                    // Some providers use "text" or "delta"
                    if (first.TryGetProperty("text", out var text))
                        return text.GetString() ?? string.Empty;
                }

                // For completions endpoints older style
                if (doc.RootElement.TryGetProperty("choices", out var choices2) && choices2.GetArrayLength() > 0)
                {
                    var c = choices2[0];
                    if (c.TryGetProperty("text", out var t))
                        return t.GetString() ?? string.Empty;
                }

                // Fallback: return whole JSON (will likely fail deserialization)
                return json;
            }
            catch
            {
                return json;
            }
        }

        private static async Task<string> SafeReadStringAsync(HttpResponseMessage response)
        {
            try
            {
                return await response.Content.ReadAsStringAsync();
            }
            catch
            {
                return string.Empty;
            }
        }

        private static string Truncate(string s, int max)
        {
            if (string.IsNullOrEmpty(s)) return s ?? string.Empty;
            return s.Length <= max ? s : s.Substring(0, max) + "...";
        }
    }
}
