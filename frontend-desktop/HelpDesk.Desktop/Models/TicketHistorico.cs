using System;

namespace HelpDesk.Desktop.Models
{
    public class TicketHistorico
    {
        public int ID_Historico { get; set; }
        public int ID_Ticket { get; set; }
        public int ID_Usuario { get; set; }
        public string Acao { get; set; } = string.Empty;
        public string? StatusAnterior { get; set; }
        public string? StatusNovo { get; set; }
        public DateTime DataHora { get; set; } = DateTime.Now;
        public string? IpOrigem { get; set; }

        public string? NomeUsuario { get; set; }
    }
}