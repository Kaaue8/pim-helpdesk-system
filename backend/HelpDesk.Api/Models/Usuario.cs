using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using HelpDesk.Api.Models;

namespace HelpDesk.Api.Models
{
    [Table("Usuarios")]
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required, MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        // Armazenará o hash da senha. NUNCA a senha em texto puro!
        [Required, MaxLength(255)]
        public string SenhaHash { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string Perfil { get; set; } = string.Empty;

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