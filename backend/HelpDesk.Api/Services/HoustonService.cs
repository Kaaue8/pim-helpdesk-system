using HelpDesk.Api.Models;
using System.Text.Json;
using System.Threading.Tasks;
using System;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.ChatCompletion;

namespace HelpDesk.Api.Services
{
    public class HoustonService
    {
        private readonly Kernel _kernel;

        public HoustonService()
        {
            // 1. Inicialização do Kernel
            string apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY") ?? throw new InvalidOperationException("OPENAI_API_KEY não configurada.");

            // Usando a sintaxe mais robusta para a criação do Kernel
            _kernel = Kernel.CreateBuilder()
                .AddOpenAIChatCompletion(
                    modelId: "gpt-4o-mini", // Modelo que suporta JSON
                    apiKey: apiKey)
                .Build();
        }

        public async Task<TriageResult> TriageTicketAsync(string title, string description)
        {
            // 2. Definir o prompt do sistema para instruir a IA
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

            // 3. Definir a mensagem do usuário (o ticket)
            string userMessage = $"Título do Ticket: {title}\nDescrição do Ticket: {description}";

            // 4. Configurar a requisição para forçar a saída JSON
            // Usamos PromptExecutionSettings para a configuração genérica
            var executionSettings = new PromptExecutionSettings
            {
                ExtensionData = new Dictionary<string, object>
                {
                    { "ResponseFormat", "json_object" }
                }
            };

            try
            {
                // 5. Chamar o modelo (CORREÇÃO: Usando KernelArguments para passar as configurações)
                var result = await _kernel.InvokePromptAsync(
                    $"{systemPrompt}\n\nTicket: {userMessage}",
                    new KernelArguments(executionSettings)); // Passa as configurações como KernelArguments

                string jsonResponse = result.GetValue<string>() ?? string.Empty;

                // 6. Processar a resposta da IA
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
                // Em caso de qualquer erro (API, rede, etc.), retorna um resultado padrão
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
