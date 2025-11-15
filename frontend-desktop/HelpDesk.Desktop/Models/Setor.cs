using System;

namespace HelpDesk.Desktop.Models
{
    public class Setor
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}