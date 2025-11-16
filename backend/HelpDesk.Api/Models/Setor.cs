using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace HelpDesk.Api.Models
{
    [Table("Setor")] // ✅ Singular, conforme banco
    public class Setor
    {
        [Key]
        [Column("ID_Setor")] // ✅ Conforme banco
        public int IdSetor { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nome { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Descricao { get; set; }

        public bool Ativo { get; set; } = true;

        // Propriedade de navegação
        public ICollection<Usuario>? Usuarios { get; set; }
    }
}

