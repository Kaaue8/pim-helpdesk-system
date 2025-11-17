using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace HelpDesk.Api.Models
{
    [Table("Setores")] // ✅ Plural, conforme banco
    public class Setor
    {
        [Key]
        [Column("id_setor")] // ✅ Conforme banco (snake_case)
        public int IdSetor { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("nome_setor")]
        public string Nome { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Descricao { get; set; }

        public bool Ativo { get; set; } = true;

        // Propriedade de navegação
        public ICollection<Usuario>? Usuarios { get; set; }
    }
}

