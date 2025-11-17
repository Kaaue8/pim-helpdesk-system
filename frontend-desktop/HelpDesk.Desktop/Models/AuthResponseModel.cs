namespace HelpDesk.Desktop.Models
{
    public class AuthResponseModel
    {
        public string Token { get; set; }
        public string NomeUsuario { get; set; }
        public string Perfil { get; set; } // Ex: "Admin", "Tecnico", "Padrao"
        // Adicione outros campos que sua API retorna no login
    }
}
