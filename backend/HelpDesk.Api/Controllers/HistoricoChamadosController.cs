using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HelpDesk.Api.Data;
using System.Security.Claims;

namespace HelpDesk.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class HistoricoChamadosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public HistoricoChamadosController(AppDbContext context)
        {
            _context = context;
        }

        private int GetUserId()
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(id, out int uid) ? uid : 0;
        }

        private string GetRole()
        {
            return User.FindFirst(ClaimTypes.Role)?.Value ?? "Usuario";
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetHistorico()
        {
            var userId = GetUserId();
            var role = GetRole();

            var query = _context.Tickets
                .Include(t => t.Usuario)
                .Where(t => t.Status == "Fechado")
                .AsQueryable();

            if (role == "Usuario")
            {
                query = query.Where(t => t.UsuarioId == userId);
            }

            var result = await query
                .Select(t => new
                {
                    t.Id,
                    t.Titulo,
                    t.Descricao,
                    t.Status,
                    t.Prioridade,
                    t.DataAbertura,
                    t.DataFechamento,
                    Usuario = t.Usuario.Nome
                })
                .ToListAsync();

            return Ok(result);
        }
    }
}
