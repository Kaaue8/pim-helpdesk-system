
namespace HelpDesk.Api.Dtos
{
    public class TicketUpdateDto
    {
        public string? Status { get; set; }
        public int? ResponsavelId { get; set; }
        public string? Solucao { get; set; }
    }
}
