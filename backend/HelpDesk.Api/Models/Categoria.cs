using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace HelpDesk.Api.Models
{
    [Table("Categoria")] // ✅ Singular, conforme banco
    public class Categoria
    {
        [Key]
        [Column("ID_Categoria")] // ✅ Conforme banco
        public int IdCategoria { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nome { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Descricao { get; set; }

        public bool Ativo { get; set; } = true;

        // Propriedade de navegação
        public ICollection<Ticket>? Tickets { get; set; }
    }
}

