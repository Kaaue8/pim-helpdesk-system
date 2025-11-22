using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HelpDesk.Api.Models;
using HelpDesk.Api.Data;

namespace HelpDesk.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasChamadosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriasChamadosController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Todas as categorias
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriasChamados>>> GetCategorias()
        {
            try
            {
                var categorias = await _context.CategoriasChamados
                    .OrderBy(c => c.Categoria)
                    .ToListAsync();

                return Ok(categorias);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro ao buscar categorias", error = ex.Message });
            }
        }

        /// <summary>
        /// Get para categoria por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriasChamados>> GetCategoria(int id)
        {
            try
            {
                var categoria = await _context.CategoriasChamados.FindAsync(id);

                if (categoria == null)
                {
                    return NotFound(new { message = "Categoria não encontrada" });
                }

                return Ok(categoria);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro ao buscar categoria", error = ex.Message });
            }
        }
    }
}
