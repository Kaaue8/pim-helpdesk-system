using System.Text.Json.Serialization;

public class TriageResultDto
{
    [JsonPropertyName("prioridade")]
    public string Prioridade { get; set; } = "Média";

    [JsonPropertyName("setorRecomendado")]
    public string SetorRecomendado { get; set; } = "Suporte";

    [JsonPropertyName("resumoTriagem")]
    public string ResumoTriagem { get; set; } = string.Empty;

    [JsonPropertyName("solucaoSugerida")]
    public string SolucaoSugerida { get; set; } = string.Empty;
}
