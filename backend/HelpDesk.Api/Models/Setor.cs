// /home/ubuntu/backend/HelpDesk.Api/Models/Setor.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace HelpDesk.Api.Models
{
    [Table("Setores")]
    public class Setor
    {
        [Key]
        [Column("id_setor")]
        public int IdSetor { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("nome_setor")]
        public required string NomeSetor { get; set; } // Adicionado 'required'

        // Propriedade de navegação para listar os usuários deste setor
        public ICollection<Usuario>? Usuarios { get; set; } // Adicionado '?' para ICollection
    }
}
