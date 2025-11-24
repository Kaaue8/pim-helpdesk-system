using System;
using HelpDesk.Desktop.Models;

namespace HelpDesk.Desktop.Services
{
    public class AuthService
    {
        private static AuthService? _instance;
        private Usuario? _currentUser;
        private string? _currentToken;
        private DateTime _loginTime;

        private AuthService() { }

        public static AuthService Instance => _instance ??= new AuthService();

        public Usuario? CurrentUser => _currentUser;
        public string? CurrentToken => _currentToken;
        public bool IsAuthenticated => _currentUser != null && !string.IsNullOrEmpty(_currentToken);

        public void SetAuthentication(Usuario usuario, string token)
        {
            _currentUser = usuario;
            _currentToken = token;
            _loginTime = DateTime.Now;
        }

        public void Logout()
        {
            _currentUser = null;
            _currentToken = null;
            ApiService.Instance.ClearAuthToken();
        }

        public bool IsAdmin()
        {
            return _currentUser?.IsAdmin() == true;
        }

        public bool IsUsuario()
        {
            return _currentUser?.IsUsuario() == true;
        }

        public bool TemPermissao(string permissao)
        {
            if (!IsAuthenticated) return false;

            return permissao.ToLower() switch
            {
                "admin" => IsAdmin(),
                "usuario" => IsAuthenticated,
                _ => false
            };
        }

        public bool IsSessionExpired(int maxMinutes = 480)
        {
            return DateTime.Now.Subtract(_loginTime).TotalMinutes > maxMinutes;
        }
    }
}