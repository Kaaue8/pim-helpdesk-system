using System;

namespace HelpDesk.Desktop.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty; // "Aberto", "Em Andamento", "Resolvido", "Fechado"
        public string Prioridade { get; set; } = string.Empty; // "Baixa", "Média", "Alta", "Urgente"
        public string Categoria { get; set; } = string.Empty;
        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
        public int? AnalistaId { get; set; }
        public Usuario? Analista { get; set; }
        public int SetorId { get; set; }
        public Setor? Setor { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
    }
}