using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HelpDesk.Api.Data;
using HelpDesk.Api.Models;
using HelpDesk.Api.Models.Dto;
using HelpDesk.Api.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace HelpDesk.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly HoustonService _houstonService;

        public TicketsController(AppDbContext context, HoustonService houstonService)
        {
            _context = context;
            _houstonService = houstonService;
        }

        private string GetUserRole() =>
            User.FindFirst(ClaimTypes.Role)?.Value ?? "Usuario";

        private int GetUserId()
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(id, out int userId) ? userId : 0;
        }

        private async Task<int> GetUserSetorId()
        {
            var userId = GetUserId();
            var usuario = await _context.Usuarios.FindAsync(userId);
            return usuario?.SetorIdSetor ?? 0;
        }

        // ============================================================
        // GET TICKETS
        // ============================================================
        [HttpGet]
        public async Task<ActionResult<object>> GetTickets()
        {
            var role = GetUserRole();
            var userId = GetUserId();

            var query = _context.Tickets
                .Include(t => t.Usuario)
                .Include(t => t.Tecnico)
                .AsQueryable();

            if (role == "Analista")
            {
                var setor = await GetUserSetorId();
                query = query.Where(t => t.Usuario.SetorIdSetor == setor);
            }
            else if (role == "Usuario")
            {
                query = query.Where(t => t.UsuarioId == userId);
            }

            var tickets = await query
                .Select(t => new
                {
                    id = t.Id,
                    titulo = t.Titulo,
                    descricao = t.Descricao,
                    status = t.Status,
                    prioridade = t.Prioridade,
                    dataAbertura = t.DataAbertura,
                    dataFechamento = t.DataFechamento,
                    setorRecomendado = t.SetorRecomendado,
                    resumoTriagem = t.ResumoTriagem,
                    solucaoSugerida = t.SolucaoSugerida,
                    solicitanteNome = t.Usuario.Nome,
                    tecnicoNome = t.Tecnico != null ? t.Tecnico.Nome : null,
                    tecnicoId = t.TecnicoId
                })
                .OrderByDescending(t => t.dataAbertura)
                .ToListAsync();

            return Ok(new { success = true, data = tickets });
        }

        // ============================================================
        // GET POR ID
        // ============================================================
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetTicket(int id)
        {
            var ticket = await _context.Tickets
                .Include(t => t.Usuario)
                .Include(t => t.Tecnico)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (ticket == null)
                return NotFound(new { success = false, message = "Ticket não encontrado." });

            var role = GetUserRole();
            var userId = GetUserId();

            if (role == "Usuario" && ticket.UsuarioId != userId)
                return Forbid();

            if (role == "Analista")
            {
                var setor = await GetUserSetorId();
                if (ticket.Usuario.SetorIdSetor != setor)
                    return Forbid();
            }

            return Ok(new
            {
                success = true,
                data = new
                {
                    id = ticket.Id,
                    titulo = ticket.Titulo,
                    descricao = ticket.Descricao,
                    status = ticket.Status,
                    prioridade = ticket.Prioridade,
                    dataAbertura = ticket.DataAbertura,
                    dataFechamento = ticket.DataFechamento,
                    setorRecomendado = ticket.SetorRecomendado,
                    resumoTriagem = ticket.ResumoTriagem,
                    solucaoSugerida = ticket.SolucaoSugerida,
                    solicitanteNome = ticket.Usuario?.Nome,
                    tecnicoNome = ticket.Tecnico?.Nome,
                    tecnicoId = ticket.TecnicoId
                }
            });
        }

        // ============================================================
        // POST
        // ============================================================
        [HttpPost]
        [Authorize(Roles = "Usuario, Admin")]
        public async Task<ActionResult<object>> PostTicket(CreateTicketDto dto)
        {
            var userId = GetUserId();
            var role = GetUserRole();

            var ticket = new Ticket
            {
                Titulo = dto.Titulo,
                Descricao = dto.Descricao,
                Status = "Aberto",
                UsuarioId = role == "Usuario" ? userId : userId,
                DataAbertura = DateTime.UtcNow
            };

            var triage = await _houstonService.TriageTicketAsync(ticket.Titulo, ticket.Descricao);

            ticket.Prioridade = triage.Prioridade;
            ticket.SetorRecomendado = triage.SetorRecomendado;
            ticket.ResumoTriagem = triage.ResumoTriagem;
            ticket.SolucaoSugerida = triage.SolucaoSugerida;

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, data = new { id = ticket.Id } });
        }

        // ============================================================
        // PUT
        // ============================================================
        [HttpPut("{id}")]
        [Authorize(Roles = "Analista, Admin")]
        public async Task<ActionResult<object>> PutTicket(int id, UpdateTicketDto dto)
        {
            var ticket = await _context.Tickets.FindAsync(id);

            if (ticket == null)
                return NotFound(new { success = false, message = "Ticket não encontrado." });

            if (dto.Id != id)
                return BadRequest(new { success = false, message = "ID inconsistente." });

            ticket.Status = dto.Status;
            ticket.Prioridade = dto.Prioridade;
            ticket.TecnicoId = dto.TecnicoId;

            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Ticket atualizado com sucesso." });
        }
    }
}
