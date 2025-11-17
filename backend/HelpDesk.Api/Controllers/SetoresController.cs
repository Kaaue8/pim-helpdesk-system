using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HelpDesk.Api.Data;
using HelpDesk.Api.Models;

namespace HelpDesk.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SetoresController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SetoresController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Setor>>> GetSetores()
        {
            var setores = await _context.Setores
                .Where(s => s.Ativo)
                .OrderBy(s => s.Nome)
                .ToListAsync();

            return Ok(setores);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Setor>> GetSetor(int id)
        {
            var setor = await _context.Setores.FindAsync(id);

            if (setor == null)
            {
                return NotFound(new { message = "Setor não encontrado" });
            }

            return Ok(setor);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Setor>> CreateSetor([FromBody] Setor setor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            setor.Ativo = true;

            _context.Setores.Add(setor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSetor), new { id = setor.IdSetor }, setor);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Setor>> UpdateSetor(int id, [FromBody] Setor setorAtualizado)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var setor = await _context.Setores.FindAsync(id);
            if (setor == null)
            {
                return NotFound(new { message = "Setor não encontrado" });
            }

            setor.Nome = setorAtualizado.Nome;
            setor.Descricao = setorAtualizado.Descricao;
            setor.Ativo = setorAtualizado.Ativo;

            await _context.SaveChangesAsync();

            return Ok(setor);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteSetor(int id)
        {
            var setor = await _context.Setores.FindAsync(id);
            if (setor == null)
            {
                return NotFound(new { message = "Setor não encontrado" });
            }

            _context.Setores.Remove(setor);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

