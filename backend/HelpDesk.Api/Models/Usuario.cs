using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelpDesk.Api.Models
{
    [Table("Usuario")]
    public class Usuario
    {
        [Key]
        [Column("ID_Usuario")]
        public int IdUsuario { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string Senha { get; set; } = string.Empty; // S maiúsculo conforme banco

        [Required]
        [MaxLength(20)]
        public string Perfil { get; set; } = "Usuario"; // Admin, Analista, Usuario

        [Column("SetorId")]
        public int? SetorId { get; set; }

        [ForeignKey("SetorId")]
        public virtual Setor? Setor { get; set; }

        [MaxLength(20)]
        public string? Telefone { get; set; }

        public bool Ativo { get; set; } = true;

        [Column("DataCriacao")]
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        // Navegação: Tickets criados por este usuário
        public virtual ICollection<Ticket> TicketsCriados { get; set; } = new List<Ticket>();

        // Navegação: Tickets atribuídos a este usuário (se for analista)
        public virtual ICollection<Ticket> TicketsAtribuidos { get; set; } = new List<Ticket>();
    }
}

