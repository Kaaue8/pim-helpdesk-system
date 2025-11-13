// /home/ubuntu/backend/HelpDesk.Api/Models/Dto/CreateUsuarioDto.cs
using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Api.Models.Dto
{
    public class CreateUsuarioDto
    {
        [Required, MaxLength(100)]
        public required string Nome { get; set; } // Adicionado 'required'

        [Required, EmailAddress, MaxLength(150)]
        public required string Email { get; set; } // Adicionado 'required'

        [Required, MinLength(6), MaxLength(100)]
        public required string Senha { get; set; } // Adicionado 'required'

        [Required, MaxLength(50)]
        public required string Perfil { get; set; } // Adicionado 'required'

        public int SetorIdSetor { get; set; }
    }
}
