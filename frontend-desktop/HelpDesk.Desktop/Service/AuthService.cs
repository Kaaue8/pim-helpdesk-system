using HelpDesk.Desktop.Models;

namespace HelpDesk.Desktop.Services
{
    /// <summary>
    /// Serviço de autenticação e gerenciamento de sessão (Singleton)
    /// </summary>
    public class AuthService
    {
        private static AuthService? _instance;
        private static readonly object _lock = new object();

        /// <summary>
        /// Instância única do serviço (Singleton)
        /// </summary>
        public static AuthService Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        _instance ??= new AuthService();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Usuário atualmente logado
        /// </summary>
        public Usuario? UsuarioLogado { get; private set; }

        /// <summary>
        /// Alias para UsuarioLogado (para compatibilidade)
        /// </summary>
        public Usuario? CurrentUser => UsuarioLogado;

        /// <summary>
        /// Token JWT de autenticação
        /// </summary>
        public string? Token { get; private set; }

        /// <summary>
        /// Verifica se há um usuário autenticado
        /// </summary>
        public bool IsAuthenticated => UsuarioLogado != null && !string.IsNullOrEmpty(Token);

        /// <summary>
        /// Data/hora do login
        /// </summary>
        public DateTime? LoginTime { get; private set; }

        // Construtor privado (Singleton)
        private AuthService() { }

        /// <summary>
        /// Define a autenticação do usuário
        /// </summary>
        public void SetAuthentication(Usuario usuario, string token)
        {
            if (usuario == null)
                throw new ArgumentNullException(nameof(usuario));

            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Token não pode ser vazio", nameof(token));

            UsuarioLogado = usuario;
            Token = token;
            LoginTime = DateTime.Now;
        }

        /// <summary>
        /// Faz logout do usuário
        /// </summary>
        public void Logout()
        {
            Token = null;
            UsuarioLogado = null;
            LoginTime = null;
        }

        /// <summary>
        /// Verifica se o usuário é Admin
        /// </summary>
        public bool IsAdmin()
        {
            return UsuarioLogado?.Perfil?.Equals("Admin", StringComparison.OrdinalIgnoreCase) == true;
        }

        /// <summary>
        /// Verifica se o usuário é Usuário comum
        /// </summary>
        public bool IsUsuario()
        {
            return UsuarioLogado?.Perfil?.Equals("Usuario", StringComparison.OrdinalIgnoreCase) == true;
        }

        /// <summary>
        /// Verifica se o usuário tem um perfil específico
        /// </summary>
        public bool TemPermissao(string perfil)
        {
            if (string.IsNullOrWhiteSpace(perfil))
                return false;

            // Admin tem acesso a tudo
            if (IsAdmin())
                return true;

            return UsuarioLogado?.Perfil?.Equals(perfil, StringComparison.OrdinalIgnoreCase) == true;
        }

        /// <summary>
        /// Obtém o ID do usuário logado
        /// </summary>
        public int GetUsuarioId()
        {
            return UsuarioLogado?.ID_Usuario ?? 0;
        }

        /// <summary>
        /// Obtém o nome do usuário logado
        /// </summary>
        public string GetUsuarioNome()
        {
            return UsuarioLogado?.Nome ?? "Usuário";
        }

        /// <summary>
        /// Obtém o email do usuário logado
        /// </summary>
        public string GetUsuarioEmail()
        {
            return UsuarioLogado?.Email ?? "";
        }

        /// <summary>
        /// Obtém o perfil do usuário logado
        /// </summary>
        public string GetUsuarioPerfil()
        {
            return UsuarioLogado?.Perfil ?? "Usuario";
        }

        /// <summary>
        /// Verifica se a sessão expirou (após 8 horas)
        /// </summary>
        public bool IsSessionExpired()
        {
            if (!IsAuthenticated || LoginTime == null)
                return true;

            var sessionDuration = DateTime.Now - LoginTime.Value;
            return sessionDuration.TotalHours > 8;
        }

        /// <summary>
        /// Renova o token de autenticação
        /// </summary>
        public void RenewToken(string newToken)
        {
            if (string.IsNullOrWhiteSpace(newToken))
                throw new ArgumentException("Token não pode ser vazio", nameof(newToken));

            Token = newToken;
        }
    }
}

