using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelpDesk.Api.Models
{
    [Table("Tickets")]
    public class Ticket
    {
        [Key]
        [Column("ID_Ticket")]
        public int Id { get; set; }

        // --- Propriedades do Ticket ---
        [Required, MaxLength(100)]
        public string Titulo { get; set; } = string.Empty;

        [Required]
        public string Descricao { get; set; } = string.Empty;

        [Required, MaxLength(20)]
        public string Status { get; set; } = string.Empty;

        [Required, MaxLength(20)]
        public string Prioridade { get; set; } = string.Empty;

        public DateTime DataAbertura { get; set; }
        public DateTime? DataFechamento { get; set; }

        // --- Campos da IA ---
        [MaxLength(50)]
        public string? SetorRecomendado { get; set; }
        public string? ResumoTriagem { get; set; }
        public string? SolucaoSugerida { get; set; }

        // ==================================================================
        // CORREÇÃO DEFINITIVA BD
        // ==================================================================

        // Mapeia a propriedade 'UsuarioId' para a coluna 'SolicitanteId' no banco
        [Column("SolicitanteId")]
        public int UsuarioId { get; set; }

        // Mapeia a propriedade 'TecnicoId' para a coluna 'ResponsavelId' no banco
        [Column("ResponsavelId")]
        public int? TecnicoId { get; set; }

        // --- Relacionamentos (Chaves Estrangeiras) ---

        // Define o relacionamento com o Usuário (Solicitante)
        [ForeignKey(nameof(UsuarioId))]
        public Usuario? Usuario { get; set; }

        // Define o relacionamento com o Usuário (Técnico Responsável)
        [ForeignKey(nameof(TecnicoId))]
        public Usuario? Tecnico { get; set; }
    }
}
