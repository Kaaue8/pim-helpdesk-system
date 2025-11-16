using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace HelpDesk.Api.Models
{
    [Table("Usuario")] // ✅ Singular, conforme banco
    public class Usuario
    {
        [Key]
        [Column("ID_Usuario")] // ✅ Conforme banco
        public int IdUsuario { get; set; } // ✅ Propriedade em PascalCase

        [Required, MaxLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required, MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        // ✅ Campo Senha com S maiúsculo, conforme banco
        [Required, MaxLength(255)]
        public string Senha { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string Perfil { get; set; } = string.Empty; // Admin ou Usuario (Admin removido)

        [Column("DataCriacao")]
        public DateTime DataCriacao { get; set; }

        // ✅ Chave estrangeira correta
        [Column("SetorId")]
        public int? SetorId { get; set; }

        [ForeignKey(nameof(SetorId))]
        public Setor? Setor { get; set; }

        [MaxLength(20)]
        public string? Telefone { get; set; }

        public bool Ativo { get; set; } = true;

        // Propriedades de navegação
        public ICollection<Ticket>? TicketsAbertos { get; set; }
        public ICollection<Ticket>? TicketsAtribuidos { get; set; }
        public ICollection<LogAuditoria>? LogsAfetados { get; set; }
        public ICollection<LogAuditoria>? LogsExecutados { get; set; }
    }
}

