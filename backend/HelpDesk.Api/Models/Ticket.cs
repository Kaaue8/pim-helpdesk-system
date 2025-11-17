using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace HelpDesk.Api.Models
{
    [Table("Tickets")] // ✅ Plural, conforme banco
    public class Ticket
    {
        [Key]
        [Column("ID_Ticket")] // ✅ Conforme banco
        public int IdTicket { get; set; }

        [Required, MaxLength(200)]
        public string Titulo { get; set; } = string.Empty;

        [Required]
        public string Descricao { get; set; } = string.Empty;

        [Required, MaxLength(20)]
        public string Status { get; set; } = "Aberto";

        [Required, MaxLength(20)]
        public string Prioridade { get; set; } = "Media";

        // ✅ Solicitante (quem abriu o ticket)
        [Column("SolicitanteId")]
        public int SolicitanteId { get; set; }

        [ForeignKey(nameof(SolicitanteId))]
        public Usuario? Solicitante { get; set; }

        // ✅ Responsável (Admin que está atendendo)
        [Column("ResponsavelId")]
        public int? ResponsavelId { get; set; }

        [ForeignKey(nameof(ResponsavelId))]
        public Usuario? Responsavel { get; set; }

        // ✅ Categoria
        [Column("ID_Categoria")]
        public int? CategoriaId { get; set; }

        [ForeignKey(nameof(CategoriaId))]
        public Categoria? Categoria { get; set; }

        [Column("DataAbertura")]
        public DateTime DataAbertura { get; set; } = DateTime.Now;

        [Column("DataFechamento")]
        public DateTime? DataFechamento { get; set; }

        [Column("DataAtualizacao")]
        public DateTime DataAtualizacao { get; set; } = DateTime.Now;

        // ✅ Solução fornecida
        public string? Solucao { get; set; }

        // ✅ Campos de Triagem da IA (mantidos)
        [MaxLength(50)]
        public string? SetorRecomendado { get; set; }

        public string? ResumoTriagem { get; set; }

        public string? SolucaoSugerida { get; set; }

        // ✅ Histórico de alterações
        public ICollection<TicketHistorico>? Historico { get; set; }
    }
}

