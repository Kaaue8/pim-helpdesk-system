using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace HelpDesk.Api.Models
{
    [Table("Usuarios")]
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public required string Nome { get; set; }

        [Required, MaxLength(150)]
        public required string Email { get; set; }

        // Armazenará o hash da senha. NUNCA a senha em texto puro!
        [Required, MaxLength(255)]
        public required string SenhaHash { get; set; }

        [Required, MaxLength(50)]
        public required string Perfil { get; set; }

        // Data de criação do registro. Removido o valor padrão dinâmico (DateTime.UtcNow)
        public DateTime DataCriacao { get; set; }

        // Chave estrangeira para Setor
        public int SetorIdSetor { get; set; }

        [ForeignKey(nameof(SetorIdSetor))]
        public Setor? Setor { get; set; }

        // Propriedades de navegação (adicionadas '?' para evitar CS8618)
        public ICollection<Ticket>? TicketsAbertos { get; set; }
        public ICollection<Ticket>? TicketsAtribuidos { get; set; }
        public ICollection<LogAuditoria>? LogsAfetados { get; set; }
        public ICollection<LogAuditoria>? LogsExecutados { get; set; }
    }
}