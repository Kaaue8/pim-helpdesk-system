using System;

namespace HelpDesk.Desktop.Models
{
    public class Usuario
    {
        public int ID_Usuario { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? SenhaHash { get; set; }
        public string Perfil { get; set; } = "Usuario";
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public int? ID_Setor { get; set; }
        public string? NomeSetor { get; set; }

        public bool IsAdmin() => Perfil?.Equals("Admin", StringComparison.OrdinalIgnoreCase) == true;
        public bool IsUsuario() => Perfil?.Equals("Usuario", StringComparison.OrdinalIgnoreCase) == true;
    }
}