using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HelpDesk.Api.Data;

namespace HelpDesk.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<object>> GetDashboard()
        {
            var total = await _context.Tickets.CountAsync();
            var abertos = await _context.Tickets.CountAsync(t => t.Status == "Aberto");
            var andamento = await _context.Tickets.CountAsync(t => t.Status == "Em Andamento");
            var fechados = await _context.Tickets.CountAsync(t => t.Status == "Fechado");

            var porSetor = await _context.Tickets
                .GroupBy(t => t.SetorRecomendado)
                .Select(g => new { Setor = g.Key, Quantidade = g.Count() })
                .ToListAsync();

            var porPrioridade = await _context.Tickets
                .GroupBy(t => t.Prioridade)
                .Select(g => new { Prioridade = g.Key, Quantidade = g.Count() })
                .ToListAsync();

            var ultimos = await _context.Tickets
                .OrderByDescending(t => t.DataAbertura)
                .Take(5)
                .Select(t => new
                {
                    t.Id,
                    t.Titulo,
                    t.Status,
                    t.Prioridade,
                    t.DataAbertura
                })
                .ToListAsync();

            return Ok(new
            {
                TotalTickets = total,
                Abertos = abertos,
                EmAndamento = andamento,
                Fechados = fechados,
                PorSetor = porSetor,
                PorPrioridade = porPrioridade,
                Ultimos = ultimos
            });
        }
    }
}
