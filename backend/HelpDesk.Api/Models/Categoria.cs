using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelpDesk.Api.Models
{
    [Table("Categoria")]
    public class Categoria
    {
        [Key]
        [Column("ID_Categoria")]
        public int IdCategoria { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nome { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Descricao { get; set; }

        public bool Ativo { get; set; } = true;

        // Navegação: Tickets desta categoria
        public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}

