using HelpDesk.Api.Data;
using HelpDesk.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        // GET: api/CategoriasChamados
        [HttpGet]
        public async Task<IActionResult> GetCategorias()
        {
            var categorias = await _context.CategoriasChamados
                .Select(c => new
                {
                    id = c.IdCategoria,
                    categoria = c.Categoria,
                    sla = c.SLA,
                    nivel = c.Nivel,
                    prioridade = c.Prioridade,
                    palavrasChave = c.PalavrasChave,
                    termosNegativos = c.TermosNegativos,
                    exemploDescricao = c.ExemploDescricao,
                    sugestaoSolucao = c.SugestaoSolucao,
                    passosSolucao = c.PassosSolucao,
                    artigoBaseConhecimento = c.ArtigoBaseConhecimento
                })
                .ToListAsync();

            return Ok(new { success = true, data = categorias });
        }

        // GET: api/CategoriasChamados/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoria(int id)
        {
            var categoria = await _context.CategoriasChamados.FindAsync(id);
            if (categoria == null)
                return NotFound(new { success = false, message = "Categoria não encontrada." });

            return Ok(new { success = true, data = categoria });
        }

        // POST: api/CategoriasChamados
        [HttpPost]
        public async Task<IActionResult> CreateCategoria(CategoriasChamados model)
        {
            _context.CategoriasChamados.Add(model);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, data = model });
        }

        // PUT: api/CategoriasChamados/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategoria(int id, CategoriasChamados model)
        {
            if (id != model.IdCategoria)
                return BadRequest(new { success = false, message = "ID inválido." });

            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { success = true });
        }

        // DELETE: api/CategoriasChamados/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoria(int id)
        {
            var categoria = await _context.CategoriasChamados.FindAsync(id);
            if (categoria == null)
                return NotFound(new { success = false, message = "Categoria não encontrada." });

            _context.CategoriasChamados.Remove(categoria);
            await _context.SaveChangesAsync();

            return Ok(new { success = true });
        }
    }
}
