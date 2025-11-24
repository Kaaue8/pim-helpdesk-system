using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HelpDesk.Api.Data;
using System.Security.Claims;

namespace HelpDesk.Api.Controllers
{
    [Authorize(Roles = "Admin,Analista")]
    [Route("api/[controller]")]
    [ApiController]
    public class FilaChamadosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FilaChamadosController(AppDbContext context)
        {
            _context = context;
        }

        private int GetUserSetorId(int userId)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Id == userId);
            return usuario?.SetorIdSetor ?? 0;
        }

        private int GetUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(claim, out int id) ? id : 0;
        }

        private string GetRole()
        {
            return User.FindFirst(ClaimTypes.Role)?.Value ?? "Usuario";
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetFila()
        {
            var userId = GetUserId();
            var role = GetRole();

            var query = _context.Tickets
                .Include(t => t.Usuario)
                .Include(t => t.Tecnico)
                .Where(t => t.Status == "Aberto")
                .AsQueryable();

            if (role == "Analista")
            {
                var setor = GetUserSetorId(userId);
                query = query.Where(t => t.Usuario.SetorIdSetor == setor);
            }

            var fila = await query
                .Select(t => new
                {
                    t.Id,
                    t.Titulo,
                    t.Prioridade,
                    t.SetorRecomendado,
                    t.ResumoTriagem,
                    t.SolucaoSugerida,
                    Tecnico = t.Tecnico != null ? t.Tecnico.Nome : null,
                    Usuario = t.Usuario.Nome,
                    t.DataAbertura
                })
                .OrderBy(t => t.Prioridade)
                .ToListAsync();

            return Ok(fila);
        }
    }
}
