using System;

namespace HelpDesk.Desktop.Models
{
    public class Ticket
    {
        public int ID_Ticket { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string Status { get; set; } = "Aberto";
        public string Prioridade { get; set; } = "Média";
        public DateTime DataAbertura { get; set; } = DateTime.Now;
        public DateTime? DataFechamento { get; set; }
        public int SolicitanteId { get; set; }
        public int? ResponsavelId { get; set; }
        public int? ID_Categoria { get; set; }
        public string? ResumoTriagem { get; set; }
        public string? SetorRecomendado { get; set; }
        public string? SolucaoSugerida { get; set; }

        public string? NomeSolicitante { get; set; }
        public string? NomeResponsavel { get; set; }
        public string? NomeCategoria { get; set; }

        public bool IsAberto() => Status?.Equals("Aberto", StringComparison.OrdinalIgnoreCase) == true;
        public bool IsFechado() => Status?.Equals("Fechado", StringComparison.OrdinalIgnoreCase) == true;
        public bool IsEmAndamento() => Status?.Equals("Em Andamento", StringComparison.OrdinalIgnoreCase) == true;
    }
}