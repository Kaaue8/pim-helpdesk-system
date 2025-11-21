using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HelpDesk.Api.Data;
using HelpDesk.Api.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using HelpDesk.Api.Services;
using HelpDesk.Api.Dtos;

namespace HelpDesk.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly HoustonService _houstonService; // Adicionado para injeção

        public TicketsController(AppDbContext context, HoustonService houstonService) // Injeção do HoustonService
        {
            _context = context;
            _houstonService = houstonService;
        }

        // --- Métodos Auxiliares para RBAC ---

        private string GetUserRole()
        {
            // O Perfil (Role) é armazenado no ClaimTypes.Role
            return User.FindFirst(ClaimTypes.Role)?.Value ?? "Usuario";
        }

        private int GetUserId()
        {
            // O ID do usuário é armazenado no ClaimTypes.NameIdentifier
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(idClaim, out int userId) ? userId : 0;
        }

        private async Task<int> GetUserSetorId()
        {
            var userId = GetUserId();
            if (userId == 0) return 0;

            var usuario = await _context.Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId);

            return usuario?.SetorIdSetor ?? 0;
        }

        // --- Endpoints ---

        // GET: api/Tickets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetTickets()
        {
            var role = GetUserRole();
            var userId = GetUserId();
            var query = _context.Tickets.AsQueryable();

            switch (role)
            {
                case "Admin":
                    // Admin vê todos os tickets
                    break;

                case "Analista":
                    // Analista vê tickets do seu setor
                    var setorId = await GetUserSetorId();
                    if (setorId > 0)
                    {
                        // Filtra tickets onde o Usuario que abriu pertence ao mesmo setor do Analista
                        query = query.Where(t => t.Usuario != null && t.Usuario.SetorIdSetor == setorId);
                    }
                    break;

                case "Usuario":
                    // Usuário comum vê apenas os tickets que ele abriu
                    query = query.Where(t => t.UsuarioId == userId);
                    break;

                default:
                    // Caso de segurança: se não for reconhecido, não vê nada
                    return Forbid();
            }

            // ✅ CORREÇÃO: Retorna dados necessários + técnico responsável
            var tickets = await query
                .Select(t => new
                {
                    t.Id,
                    t.Titulo,
                    t.Descricao,
                    t.Status,
                    t.Prioridade,
                    t.DataAbertura,
                    t.DataFechamento,
                    t.SetorRecomendado,
                    t.ResumoTriagem,
                    t.SolucaoSugerida,
                    SolicitanteNome = t.Usuario.Nome,
                    SolicitanteEmail = t.Usuario.Email,
                    TecnicoNome = t.Tecnico != null ? t.Tecnico.Nome : null,  // ✅ NOVO: Técnico responsável
                    TecnicoId = t.TecnicoId  // ✅ NOVO: ID do técnico
                })
                .ToListAsync();

            return Ok(tickets);
        }

        // GET: api/Tickets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetTicket(int id)
        {
            var ticket = await _context.Tickets
                .Include(t => t.Usuario)
                .Include(t => t.Tecnico)  // ✅ NOVO: Inclui o técnico
                .FirstOrDefaultAsync(t => t.Id == id);

            if (ticket == null)
            {
                return NotFound();
            }

            // Lógica de Autorização para visualização
            var role = GetUserRole();
            var userId = GetUserId();

            if (role == "Usuario" && ticket.UsuarioId != userId)
            {
                return Forbid(); // Usuário só pode ver seus próprios tickets
            }

            if (role == "Analista")
            {
                var analistaSetorId = await GetUserSetorId();
                if (ticket.Usuario?.SetorIdSetor != analistaSetorId)
                {
                    // Analista só pode ver tickets do seu setor
                    return Forbid();
                }
            }

            // ✅ CORREÇÃO: Retorna dados necessários + técnico responsável
            var result = new
            {
                ticket.Id,
                ticket.Titulo,
                ticket.Descricao,
                ticket.Status,
                ticket.Prioridade,
                ticket.DataAbertura,
                ticket.DataFechamento,
                ticket.SetorRecomendado,
                ticket.ResumoTriagem,
                ticket.SolucaoSugerida,
                SolicitanteNome = ticket.Usuario?.Nome,
                SolicitanteEmail = ticket.Usuario?.Email,
                TecnicoNome = ticket.Tecnico?.Nome,  // ✅ NOVO: Técnico responsável
                TecnicoId = ticket.TecnicoId  // ✅ NOVO: ID do técnico
            };

            return Ok(result);
        }

        // POST: api/Tickets
        [HttpPost]
        [Authorize(Roles = "Usuario, Admin")] // Apenas Usuário Comum e Admin podem abrir tickets
        public async Task<ActionResult<Ticket>> PostTicket(Ticket ticket)
        {
            var userId = GetUserId();
            var role = GetUserRole();

            // 1. Força o UsuarioId do ticket a ser o ID do usuário logado (Segurança)
            if (role == "Usuario")
            {
                ticket.UsuarioId = userId;
            }
            // Se for Admin, o Admin pode abrir um ticket para outro usuário (se o UsuarioId for fornecido)
            // Se o Admin não fornecer, forçamos para o ID do Admin
            else if (role == "Admin" && ticket.UsuarioId == 0)
            {
                ticket.UsuarioId = userId;
            }

            // Validação básica: O usuário que abre o ticket deve existir
            if (!_context.Usuarios.Any(u => u.Id == ticket.UsuarioId))
            {
                return BadRequest("O UsuárioId fornecido não existe.");
            }

            // Define o status inicial e a data de abertura
            ticket.Status = "Aberto";
            ticket.DataAbertura = DateTime.UtcNow;

            // --- 2. Triagem com a IA (Houston) ---
            var triageResult = await _houstonService.TriageTicketAsync(ticket.Titulo, ticket.Descricao);

            // 3. Aplicar resultados da triagem ao ticket
            ticket.Prioridade = triageResult.Prioridade;
            ticket.SetorRecomendado = triageResult.SetorRecomendado;
            ticket.ResumoTriagem = triageResult.ResumoTriagem;
            ticket.SolucaoSugerida = triageResult.SolucaoSugerida;


            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTicket), new { id = ticket.Id }, ticket);
        }

        // PUT: api/Tickets/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Analista, Admin")] // Apenas Analista e Admin podem atualizar tickets
        public async Task<IActionResult> PutTicket(int id, Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return BadRequest();
            }

            var role = GetUserRole();
            var analistaSetorId = await GetUserSetorId();

            // Carrega o ticket original para verificar a autorização
            var existingTicket = await _context.Tickets
                .Include(t => t.Usuario)
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);

            if (existingTicket == null)
            {
                return NotFound();
            }

            // 1. Verificação de Autorização para Analista
            if (role == "Analista" && existingTicket.Usuario?.SetorIdSetor != analistaSetorId)
            {
                return Forbid(); // Analista só pode atualizar tickets do seu setor
            }

            // 2. Restrição de campos para Analista
            if (role == "Analista")
            {
                // Analista só pode alterar Status, Prioridade e TecnicoId
                // O ticket que chega no body (ticket) deve ter os campos não permitidos
                // iguais aos do ticket existente (existingTicket)
                if (ticket.Titulo != existingTicket.Titulo || ticket.Descricao != existingTicket.Descricao)
                {
                    return BadRequest("Analistas só podem alterar Status, Prioridade e Técnico.");
                }
            }

            // 3. Aplica a atualização
            _context.Entry(ticket).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Tickets.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
    }
}
