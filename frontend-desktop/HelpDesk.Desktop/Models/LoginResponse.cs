using System;

namespace HelpDesk.Desktop.Models
{
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public Usuario? Usuario { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string TokenType { get; set; } = "Bearer";

        public bool IsValid() => !string.IsNullOrEmpty(Token) && Usuario != null;
        public bool IsExpired() => DateTime.Now >= ExpiresAt;
    }
}