using System.Text.Json.Serialization;

public class UpdateTicketDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("prioridade")]
    public string Prioridade { get; set; } = string.Empty;

    [JsonPropertyName("tecnicoId")]
    public int? TecnicoId { get; set; }
}
