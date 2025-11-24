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

        // ============================================================
        // GET: api/CategoriasChamados
        // ============================================================
        [HttpGet]
        public async Task<ActionResult<object>> GetCategorias()
        {
            try
            {
                var categorias = await _context.CategoriasChamados
                    .OrderBy(c => c.Categoria)
                    .ToListAsync();

                var resultado = categorias.Select(c => new
                {
                    id = c.Id,
                    categoria = c.Categoria,
                    descricao = c.Descricao
                });

                return Ok(new { success = true, data = resultado });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Erro ao buscar categorias", error = ex.Message });
            }
        }

        // ============================================================
        // GET: api/CategoriasChamados/{id}
        // ============================================================
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetCategoria(int id)
        {
            try
            {
                var categoria = await _context.CategoriasChamados.FindAsync(id);

                if (categoria == null)
                {
                    return NotFound(new { success = false, message = "Categoria não encontrada" });
                }

                return Ok(new
                {
                    success = true,
                    data = new
                    {
                        id = categoria.Id,
                        categoria = categoria.Categoria,
                        descricao = categoria.Descricao
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Erro ao buscar categoria", error = ex.Message });
            }
        }

        // ============================================================
        // POST: api/CategoriasChamados
        // ============================================================
        [HttpPost]
        public async Task<ActionResult<object>> PostCategoria(CategoriasChamados dto)
        {
            try
            {
                var categoria = new CategoriasChamados
                {
                    Categoria = dto.Categoria,
                    Descricao = dto.Descricao
                };

                _context.CategoriasChamados.Add(categoria);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    data = new
                    {
                        id = categoria.Id,
                        categoria = categoria.Categoria,
                        descricao = categoria.Descricao
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Erro ao criar categoria", error = ex.Message });
            }
        }

        // ============================================================
        // PUT: api/CategoriasChamados/{id}
        // ============================================================
        [HttpPut("{id}")]
        public async Task<ActionResult<object>> PutCategoria(int id, CategoriasChamados dto)
        {
            if (id != dto.Id)
                return BadRequest(new { success = false, message = "O ID da URL não corresponde ao ID enviado." });

            var categoria = await _context.CategoriasChamados.FindAsync(id);

            if (categoria == null)
                return NotFound(new { success = false, message = "Categoria não encontrada." });

            categoria.Categoria = dto.Categoria;
            categoria.Descricao = dto.Descricao;

            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Categoria atualizada com sucesso." });
        }

        // ============================================================
        // DELETE: api/CategoriasChamados/{id}
        // ============================================================
        [HttpDelete("{id}")]
        public async Task<ActionResult<object>> DeleteCategoria(int id)
        {
            var categoria = await _context.CategoriasChamados.FindAsync(id);

            if (categoria == null)
                return NotFound(new { success = false, message = "Categoria não encontrada." });

            _context.CategoriasChamados.Remove(categoria);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Categoria removida com sucesso." });
        }
    }
}
