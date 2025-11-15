namespace HelpDesk.Desktop.Models
{
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public Usuario? Usuario { get; set; }
    }
}