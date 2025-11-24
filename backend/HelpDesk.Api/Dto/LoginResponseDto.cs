using System.Text.Json.Serialization;

namespace HelpDesk.Api.Models.Dto
{
    public class LoginResponseDto
    {
        [JsonPropertyName("token")]
        public string Token { get; set; } = string.Empty;

        [JsonPropertyName("usuarioId")]
        public int UsuarioId { get; set; }

        [JsonPropertyName("nome")]
        public string Nome { get; set; } = string.Empty;

        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("perfil")]
        public string Perfil { get; set; } = string.Empty;

        // ===== NOVAS PROPRIEDADES =====
        [JsonPropertyName("setorId")]
        public int? SetorId { get; set; }

        [JsonPropertyName("nomeSetor")]
        public string? NomeSetor { get; set; }
    }
}
