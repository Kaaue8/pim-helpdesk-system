using System.Text.Json.Serialization;

namespace HelpDesk.Api.Models
{
    /// <summary>
    /// Representa o resultado da triagem automática de um ticket realizada pela IA.
    /// </summary>
    public class TriageResultcibtub
    {
        [JsonPropertyName("prioridade")]
        public string Prioridade { get; set; } = string.Empty;

        [JsonPropertyName("setor_recomendado")]
        public string SetorRecomendado { get; set; } = string.Empty;

        [JsonPropertyName("resumo_triagem")]
        public string ResumoTriagem { get; set; } = string.Empty;

        [JsonPropertyName("solucao_sugerida")]
        public string SolucaoSugerida { get; set; } = string.Empty;
    }
}
