using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelpDesk.Api.Models
{
    [Table("Tickets")]
    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Titulo { get; set; } = string.Empty;

        [Required]
        public string Descricao { get; set; } = string.Empty;

        [Required, MaxLength(20)]
        public string Status { get; set; } = string.Empty;

        [Required, MaxLength(20)]
        public string Prioridade { get; set; } = string.Empty;

        public int UsuarioId { get; set; }

        [ForeignKey(nameof(UsuarioId))]
        public Usuario? Usuario { get; set; }

        public int? TecnicoId { get; set; }

        [ForeignKey(nameof(TecnicoId))]
        public Usuario? Tecnico { get; set; }

        // REMOVIDO o valor padrão dinâmico (= DateTime.UtcNow)
        public DateTime DataAbertura { get; set; }

        public DateTime? DataFechamento { get; set; }

        // Campos de Triagem da IA
        [MaxLength(50)]
        public string? SetorRecomendado { get; set; }

        public string? ResumoTriagem { get; set; }

        public string? SolucaoSugerida { get; set; }
    }
}