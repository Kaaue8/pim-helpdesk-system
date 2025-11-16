using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using HelpDesk.Api.Models;
using HelpDesk.Api.Services;

namespace HelpDesk.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TicketsController : ControllerBase
    {
        private readonly TicketService _ticketService;

        public TicketsController(TicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Ticket>>> GetTickets()
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var perfil = User.FindFirst(ClaimTypes.Role)?.Value ?? "Usuario";

            var tickets = await _ticketService.GetTicketsByUsuarioAsync(usuarioId, perfil);
            return Ok(tickets);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Ticket>> GetTicket(int id)
        {
            var ticket = await _ticketService.GetTicketByIdAsync(id);

            if (ticket == null)
            {
                return NotFound(new { message = "Ticket não encontrado" });
            }

            return Ok(ticket);
        }

        [HttpPost]
        public async Task<ActionResult<Ticket>> CreateTicket([FromBody] Ticket ticket)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            ticket.SolicitanteId = usuarioId;

            var novoTicket = await _ticketService.CreateTicketAsync(ticket);

            return CreatedAtAction(nameof(GetTicket), new { id = novoTicket.IdTicket }, novoTicket);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Ticket>> UpdateTicket(int id, [FromBody] Ticket ticket)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ticketAtualizado = await _ticketService.UpdateTicketAsync(id, ticket);

            if (ticketAtualizado == null)
            {
                return NotFound(new { message = "Ticket não encontrado" });
            }

            return Ok(ticketAtualizado);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteTicket(int id)
        {
            var sucesso = await _ticketService.DeleteTicketAsync(id);

            if (!sucesso)
            {
                return NotFound(new { message = "Ticket não encontrado" });
            }

            return NoContent();
        }
    }
}

