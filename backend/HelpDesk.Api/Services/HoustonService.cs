// ARQUIVO: HoustonService.cs (VERSÃO FINAL E CORRIGIDA)

using HelpDesk.Api.Models;
using System.Text.Json;
using System.Threading.Tasks;
using System;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.Extensions.Configuration; // 1. ADICIONE ESTE 'using'

namespace HelpDesk.Api.Services
{
    public class HoustonService
    {
        private readonly Kernel _kernel;

        // 2. O CONSTRUTOR AGORA RECEBE IConfiguration
        public HoustonService(IConfiguration configuration)
        {
            // 3. LÊ A CHAVE DA FORMA CORRETA (de appsettings.json ou user-secrets)
            string apiKey = configuration["OPENAI_API_KEY"];

            // 4. VERIFICA SE A CHAVE FOI ENCONTRADA
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new InvalidOperationException("A chave OPENAI_API_KEY não foi encontrada nas configurações.");
            }

            // O resto da sua lógica de inicialização permanece o mesmo
            _kernel = Kernel.CreateBuilder()
                .AddOpenAIChatCompletion(
                    modelId: "gpt-4o-mini",
                    apiKey: apiKey)
                .Build();
        }

        // 5. TODO O RESTO DO SEU CÓDIGO (TriageTicketAsync) PERMANECE EXATAMENTE IGUAL.
        // Não precisa mudar nada aqui.
        public async Task<TriageResult> TriageTicketAsync(string title, string description)
        {
            string systemPrompt = $@"Você é o assistente de triagem de chamados 'Houston'. Sua função é analisar o título e a descrição de um ticket e retornar um objeto JSON com a prioridade, o setor responsável, um resumo da triagem e uma sugestão de solução.

Regras:
- A prioridade deve ser 'Alta', 'Média' ou 'Baixa'.
- O setor recomendado deve ser um dos seguintes: 'TI', 'Financeiro', 'Recursos Humanos', 'Comercial' ou 'Suporte'.
- A resposta DEVE ser um objeto JSON válido que se encaixe no seguinte esquema:
{{
    ""prioridade"": ""string (Alta, Média ou Baixa)"",
    ""setor_recomendado"": ""string (TI, Financeiro, Recursos Humanos, Comercial ou Suporte)"",
    ""resumo_triagem"": ""string (Breve explicação da IA sobre a triagem)"",
    ""solucao_sugerida"": ""string (Sugestão de solução para o analista)""
}}";

            string userMessage = $"Título do Ticket: {title}\nDescrição do Ticket: {description}";

            var executionSettings = new PromptExecutionSettings
            {
                ExtensionData = new Dictionary<string, object>
                {
                    { "ResponseFormat", "json_object" }
                }
            };

            try
            {
                var result = await _kernel.InvokePromptAsync(
                    $"{systemPrompt}\n\nTicket: {userMessage}",
                    new KernelArguments(executionSettings));

                string jsonResponse = result.GetValue<string>() ?? string.Empty;

                var triageResult = JsonSerializer.Deserialize<TriageResult>(jsonResponse, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return triageResult ?? new TriageResult
                {
                    Prioridade = "Média",
                    SetorRecomendado = "Suporte",
                    ResumoTriagem = "Falha na triagem automática. Conteúdo JSON inválido.",
                    SolucaoSugerida = "Verificar manualmente."
                };
            }
            catch (Exception ex)
            {
                return new TriageResult
                {
                    Prioridade = "Média",
                    SetorRecomendado = "Suporte",
                    ResumoTriagem = $"Erro na comunicação com a IA: {ex.Message}",
                    SolucaoSugerida = "Verificar o log da API."
                };
            }
        }
    }
}
