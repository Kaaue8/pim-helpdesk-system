using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelpDesk.Api.Models
{
    [Table("Ticket")]
    public class Ticket
    {
        [Key]
        [Column("ID_Ticket")]
        public int IdTicket { get; set; }

        [Required]
        [MaxLength(200)]
        public string Titulo { get; set; } = string.Empty;

        [Required]
        public string Descricao { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Prioridade { get; set; } = "Media"; // Baixa, Media, Alta, Urgente

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Aberto"; // Aberto, EmAndamento, Resolvido, Fechado

        [Column("CategoriaId")]
        public int? CategoriaId { get; set; }

        [ForeignKey("CategoriaId")]
        public virtual Categoria? Categoria { get; set; }

        [Column("SolicitanteId")]
        public int SolicitanteId { get; set; }

        [ForeignKey("SolicitanteId")]
        public virtual Usuario Solicitante { get; set; } = null!;

        [Column("ResponsavelId")]
        public int? ResponsavelId { get; set; }

        [ForeignKey("ResponsavelId")]
        public virtual Usuario? Responsavel { get; set; }

        public string? Solucao { get; set; }

        [Column("DataAbertura")]
        public DateTime DataAbertura { get; set; } = DateTime.Now;

        [Column("DataFechamento")]
        public DateTime? DataFechamento { get; set; }

        [Column("DataAtualizacao")]
        public DateTime DataAtualizacao { get; set; } = DateTime.Now;
    }
}

