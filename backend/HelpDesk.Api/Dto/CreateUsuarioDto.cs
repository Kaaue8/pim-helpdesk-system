using System.Text.Json.Serialization;

public class CreateUsuarioDto
{
    [JsonPropertyName("Nome")]
    public string Nome { get; set; } = string.Empty;

    [JsonPropertyName("Email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("Senha")]
    public string Senha { get; set; } = string.Empty;

    [JsonPropertyName("Perfil")]
    public string Perfil { get; set; } = string.Empty;

    [JsonPropertyName("SetorIdSetor")]
    public int SetorIdSetor { get; set; }
}
