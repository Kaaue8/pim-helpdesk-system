using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelpDesk.Api.Models
{
    [Table("LogsAuditoria")]
    public class LogAuditoria
    {
        [Key, Column("id_log")]
        public int IdLog { get; set; }

        [Column("id_usuario_afetado")]
        public int IdUsuarioAfetado { get; set; }

        [ForeignKey(nameof(IdUsuarioAfetado))]
        public Usuario? UsuarioAfetado { get; set; }

        [Column("id_usuario_executor")]
        public int IdUsuarioExecutor { get; set; }

        [ForeignKey(nameof(IdUsuarioExecutor))]
        public Usuario? UsuarioExecutor { get; set; }

        [Required, MaxLength(100), Column("acao")]
        public required string Acao { get; set; }

        [MaxLength(100), Column("tabela_afetada")]
        public string? TabelaAfetada { get; set; }

        // REMOVIDO o valor padrão dinâmico (= DateTime.UtcNow)
        [Column("data_hora")]
        public DateTime DataHora { get; set; }

        [MaxLength(50), Column("ip_origem")]
        public string? IpOrigem { get; set; }
    }
}