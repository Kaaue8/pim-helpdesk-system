using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelpDesk.Api.Models
{
    [Table("TicketHistorico")]
    public class TicketHistorico
    {
        [Key]
        [Column("ID_Historico")]
        public int IdHistorico { get; set; }

        [Column("ID_Ticket")]
        public int TicketId { get; set; }

        [ForeignKey(nameof(TicketId))]
        public Ticket? Ticket { get; set; }

        [Column("ID_Usuario")]
        public int UsuarioId { get; set; }

        [ForeignKey(nameof(UsuarioId))]
        public Usuario? Usuario { get; set; }

        [Required, MaxLength(50)]
        public string Acao { get; set; } = string.Empty; // Criado, Atualizado, Atribuido, Resolvido, Fechado

        [Required]
        public string Descricao { get; set; } = string.Empty; // Descrição da mudança

        public string? StatusAnterior { get; set; }
        public string? StatusNovo { get; set; }

        public string? PrioridadeAnterior { get; set; }
        public string? PrioridadeNova { get; set; }

        public int? ResponsavelAnteriorId { get; set; }
        public int? ResponsavelNovoId { get; set; }

        [Column("DataHora")]
        public DateTime DataHora { get; set; } = DateTime.Now;

        [MaxLength(50)]
        public string? IpOrigem { get; set; }
    }
}

