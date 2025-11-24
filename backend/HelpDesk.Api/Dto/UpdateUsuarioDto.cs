using System.Text.Json.Serialization;

public class UpdateUsuarioDto
{
    [JsonPropertyName("Nome")]
    public string Nome { get; set; } = string.Empty;

    [JsonPropertyName("Email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("Perfil")]
    public string Perfil { get; set; } = string.Empty;

    [JsonPropertyName("SetorIdSetor")]
    public int SetorIdSetor { get; set; }

    // Front sometimes sends senhaHash (placeholder). Aceite opcionalmente.
    [JsonPropertyName("senhaHash")]
    public string? SenhaHash { get; set; }
}
