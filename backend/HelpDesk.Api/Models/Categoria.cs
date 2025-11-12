// /home/ubuntu/backend/HelpDesk.Api/Models/Categoria.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelpDesk.Api.Models
{
    [Table("Categorias")]
    public class Categoria
    {
        [Key]
        [Column("id_categoria")]
        public int IdCategoria { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("nome_categoria")]
        public required string NomeCategoria { get; set; } // Adicionado 'required'
    }
}
