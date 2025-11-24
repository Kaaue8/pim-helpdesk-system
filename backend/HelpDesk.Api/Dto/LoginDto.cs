using System.Text.Json.Serialization;

public class LoginDto
{
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;
    [JsonPropertyName("Senha")]
    public string Senha { get; set; } = string.Empty;
}
