using System.Text.Json.Serialization;

public class CreateTicketDto
{
    [JsonPropertyName("titulo")]
    public string Titulo { get; set; } = string.Empty;

    [JsonPropertyName("descricao")]
    public string Descricao { get; set; } = string.Empty;

    [JsonPropertyName("prioridade")]
    public string Prioridade { get; set; } = "media";

    [JsonPropertyName("status")]
    public string Status { get; set; } = "Aberto";
}
