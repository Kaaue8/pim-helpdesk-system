using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelpDesk.Api.Models
{
    [Table("CategoriasChamados")]
    public class CategoriasChamados
    {
        [Key]
        [Column("ID_Categoria2")]
        public int IdCategoria { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("Categoria")]
        public required string Categoria { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("SLA")]
        public required string SLA { get; set; }

        [Required]
        [MaxLength(10)]
        [Column("Nivel")]
        public required string Nivel { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("Prioridade")]
        public required string Prioridade { get; set; }

        [MaxLength(500)]
        [Column("PalavrasChave")]
        public string? PalavrasChave { get; set; }

        [MaxLength(500)]
        [Column("TermosNegativos")]
        public string? TermosNegativos { get; set; }

        [MaxLength(500)]
        [Column("ExemploDescricao")]
        public string? ExemploDescricao { get; set; }

        [MaxLength(1000)]
        [Column("SugestaoSolucao")]
        public string? SugestaoSolucao { get; set; }

        [MaxLength(2000)]
        [Column("PassosSolucao")]
        public string? PassosSolucao { get; set; }

        [MaxLength(200)]
        [Column("ArtigoBaseConhecimento")]
        public string? ArtigoBaseConhecimento { get; set; }
    }
}
