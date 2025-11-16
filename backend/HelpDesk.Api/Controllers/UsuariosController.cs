using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HelpDesk.Api.Data;
using HelpDesk.Api.Models;
using HelpDesk.Api.Models.Dto;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;

namespace HelpDesk.Api.Controllers
{
    [Authorize] // Adicionado para proteger o Controller
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;

            // Lógica de inicialização de dados de teste removida do construtor
            // para seguir as boas práticas do ASP.NET Core.
        }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            // Não retornar o hash da senha
            var usuarios = await _context.Usuarios.ToListAsync();
            usuarios.ForEach(u => u.SenhaHash = string.Empty);
            return usuarios;
        }

        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            // Não retornar o hash da senha
            usuario.SenhaHash = string.Empty;

            return usuario;
        }

        // POST: api/Usuarios
        // Agora recebe um DTO para separar a senha do modelo de banco de dados
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(CreateUsuarioDto dto)
        {
            // 1. Validação do Modelo (ModelState.IsValid)
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // 2. Criação do objeto Usuario a partir do DTO
            var usuario = new Usuario
            {
                Nome = dto.Nome,
                Email = dto.Email,
                Perfil = dto.Perfil,
                SetorId = dto.SetorId,
                DataCriacao = System.DateTime.UtcNow,
                SenhaHash = string.Empty // Inicializa o membro requerido
            };

            // 3. Hashing da Senha (LGPD Compliance)
            usuario.SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha);

            // 4. Adiciona o usuário ao contexto e salva no banco de dados
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            // 5. Retorna o usuário criado com o status 201 Created
            // Limpa o SenhaHash antes de retornar para evitar exposição
            usuario.SenhaHash = string.Empty;

            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.IdUsuario }, usuario);
        }

        // PUT: api/Usuarios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
        {
            if (id != usuario.IdUsuario)
            {
                return BadRequest();
            }

            // Lógica de PUT simplificada:
            // Se a senha for alterada, ela deve ser hasheada.
            if (!string.IsNullOrEmpty(usuario.SenhaHash) && usuario.SenhaHash.Length < 60)
            {
                usuario.SenhaHash = BCrypt.Net.BCrypt.HashPassword(usuario.SenhaHash);
            }

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Usuarios.Any(e => e.IdUsuario == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Retorna o status 204 No Content
            return NoContent();
        }

        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            // Retorna o status 204 No Content
            return NoContent();
        }

    }
}