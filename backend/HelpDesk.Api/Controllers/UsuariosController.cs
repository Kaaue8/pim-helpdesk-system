using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HelpDesk.Api.Data;
using HelpDesk.Api.Models;
using HelpDesk.Api.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace HelpDesk.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        // ============================================================
        // PERFIL DO USUÁRIO LOGADO
        // ============================================================
        [HttpGet("me")]
        public async Task<ActionResult<object>> GetMe()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized(new { success = false, message = "Token inválido." });

            var usuario = await _context.Usuarios.FindAsync(int.Parse(userId));

            return Ok(new
            {
                success = true,
                data = new
                {
                    id = usuario.Id,
                    nome = usuario.Nome,
                    email = usuario.Email,
                    role = usuario.Perfil
                }
            });
        }

        // ============================================================
        // GET TODOS
        // ============================================================
        [HttpGet]
        public async Task<ActionResult<object>> GetUsuarios()
        {
            var usuarios = await _context.Usuarios.ToListAsync();

            return Ok(new
            {
                success = true,
                data = usuarios.Select(u => new
                {
                    id = u.Id,
                    nome = u.Nome,
                    email = u.Email,
                    role = u.Perfil,
                    setorIdSetor = u.SetorIdSetor,
                    dataCriacao = u.DataCriacao
                })
            });
        }

        // ============================================================
        // GET POR ID
        // ============================================================
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
                return NotFound(new { success = false, message = "Usuário não encontrado." });

            return Ok(new
            {
                success = true,
                data = new
                {
                    id = usuario.Id,
                    nome = usuario.Nome,
                    email = usuario.Email,
                    role = usuario.Perfil,
                    setorIdSetor = usuario.SetorIdSetor
                }
            });
        }

        // ============================================================
        // POST
        // ============================================================
        [HttpPost]
        public async Task<ActionResult<object>> PostUsuario(CreateUsuarioDto dto)
        {
            if (await _context.Usuarios.AnyAsync(u => u.Email == dto.Email))
                return BadRequest(new { success = false, message = "Email já está em uso." });

            var usuario = new Usuario
            {
                Nome = dto.Nome,
                Email = dto.Email,
                Perfil = dto.Perfil,
                SetorIdSetor = dto.SetorIdSetor,
                DataCriacao = DateTime.UtcNow,
                SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha)
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                data = new
                {
                    id = usuario.Id,
                    nome = usuario.Nome,
                    email = usuario.Email,
                    role = usuario.Perfil
                }
            });
        }

        // ============================================================
        // PUT
        // ============================================================
        [HttpPut("{id}")]
        public async Task<ActionResult<object>> PutUsuario(int id, UpdateUsuarioDto dto)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
                return NotFound(new { success = false, message = "Usuário não encontrado." });

            usuario.Nome = dto.Nome;
            usuario.Email = dto.Email;
            usuario.Perfil = dto.Perfil;
            usuario.SetorIdSetor = dto.SetorIdSetor;

            if (!string.IsNullOrWhiteSpace(dto.SenhaHash))
                usuario.SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.SenhaHash);

            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Usuário atualizado com sucesso." });
        }

        // ============================================================
        // DELETE
        // ============================================================
        [HttpDelete("{id}")]
        public async Task<ActionResult<object>> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
                return NotFound(new { success = false, message = "Usuário não encontrado." });

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Usuário removido com sucesso." });
        }
    }
}
