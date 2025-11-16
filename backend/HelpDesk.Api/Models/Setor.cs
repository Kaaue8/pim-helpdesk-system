using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelpDesk.Api.Models
{
    [Table("Setor")]
    public class Setor
    {
        [Key]
        [Column("ID_Setor")]
        public int IdSetor { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nome { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Descricao { get; set; }

        public bool Ativo { get; set; } = true;

        // Navegação: Usuários deste setor
        public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }
}

