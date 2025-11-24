namespace HelpDesk.Desktop.Models
{
    /// <summary>
    /// Resposta da API para autenticação (login)
    /// </summary>
    public class AuthResponse
    {
        /// <summary>
        /// Token JWT de autenticação
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Dados do usuário autenticado
        /// </summary>
        public Usuario Usuario { get; set; }
    }
}
