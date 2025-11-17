// CÓDIGO FINAL, CORRIGIDO COM BASE NO SEU PRINT DO AZURE

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace HelpDesk.Api.Models
{
    public class Usuario
    {
        [Key]
        [Column("ID_Usuario")] // Corresponde ao que vemos no seu print
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string Nome { get; set; }

        [Required]
        [StringLength(150)]
        public required string Email { get; set; }

        [Required]
        [StringLength(255)]
        public required string SenhaHash { get; set; }

        [Required]
        [StringLength(50)]
        public required string Perfil { get; set; }

        [Required]
        public DateTime DataCriacao { get; set; }

        [Required]
        [Column("ID_Setor")] // A correção final que resolve o erro do log
        public int SetorIdSetor { get; set; }

        // Propriedades de navegação para o EF funcionar corretamente
        public virtual ICollection<LogAuditoria> LogsAfetados { get; set; } = new List<LogAuditoria>();
        public virtual ICollection<LogAuditoria> LogsExecutados { get; set; } = new List<LogAuditoria>();
        public virtual ICollection<Ticket> TicketsAbertos { get; set; } = new List<Ticket>();
        public virtual ICollection<Ticket> TicketsAtribuidos { get; set; } = new List<Ticket>();

        [ForeignKey(nameof(SetorIdSetor))]
            public virtual Setor? Setor { get; set; }

    }
}
