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
        public required string Titulo { get; set; }

        [Required]
        public required string Descricao { get; set; }

        [Required, MaxLength(20)]
        public required string Status { get; set; }

        [Required, MaxLength(20)]
        public required string Prioridade { get; set; }

        public int UsuarioId { get; set; }

        [ForeignKey(nameof(UsuarioId))]
        public Usuario? Usuario { get; set; }

        public int? TecnicoId { get; set; }

        [ForeignKey(nameof(TecnicoId))]
        public Usuario? Tecnico { get; set; }

        // REMOVIDO o valor padrão dinâmico (= DateTime.UtcNow)
        public DateTime DataAbertura { get; set; }

        public DateTime? DataFechamento { get; set; }
    }
}