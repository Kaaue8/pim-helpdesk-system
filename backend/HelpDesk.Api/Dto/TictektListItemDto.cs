using System;
using System.Text.Json.Serialization;

public class TicketListItemDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("titulo")]
    public string Titulo { get; set; } = string.Empty;

    [JsonPropertyName("descricao")]
    public string Descricao { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("prioridade")]
    public string Prioridade { get; set; } = string.Empty;

    [JsonPropertyName("dataAbertura")]
    public DateTime DataAbertura { get; set; }

    [JsonPropertyName("dataFechamento")]
    public DateTime? DataFechamento { get; set; }

    [JsonPropertyName("setorRecomendado")]
    public string? SetorRecomendado { get; set; }

    [JsonPropertyName("solicitanteNome")]
    public string? SolicitanteNome { get; set; }

    [JsonPropertyName("tecnicoNome")]
    public string? TecnicoNome { get; set; }

    [JsonPropertyName("solucaoSugerida")]
    public string? SolucaoSugerida { get; set; }
}
