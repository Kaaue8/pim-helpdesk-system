using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace HelpDesk.Api.Models
{
    [Table("Categorias")] // ✅ Plural, conforme banco
    public class Categoria
    {
        [Key]
        [Column("id_categoria")] // ✅ Conforme banco (snake_case)
        public int IdCategoria { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("nome_categoria")]
        public string Nome { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Descricao { get; set; }

        public bool Ativo { get; set; } = true;

        // Propriedade de navegação
        public ICollection<Ticket>? Tickets { get; set; }
    }
}

