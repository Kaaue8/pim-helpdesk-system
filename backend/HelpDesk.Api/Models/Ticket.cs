// Este é o código COMPLETO e CORRIGIDO para o arquivo Ticket.cs

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelpDesk.Api.Models
{
    [Table("Tickets")]
    public class Ticket
    {
        [Key]
        [Column("ID_Ticket")] // Mapeia esta propriedade para a coluna "ID_Ticket" no banco de dados
        public int Id { get; set; }

        // O resto do seu código permanece o mesmo
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

        public DateTime DataAbertura { get; set; }

        public DateTime? DataFechamento { get; set; }

        [MaxLength(50)]
        public string? SetorRecomendado { get; set; }

        public string? ResumoTriagem { get; set; }

        public string? SolucaoSugerida { get; set; }
    }
}
