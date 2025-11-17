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

        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            // Por segurança, nunca retornar o hash da senha para o frontend.
            var usuarios = await _context.Usuarios.ToListAsync();
            usuarios.ForEach(u => u.SenhaHash = string.Empty); // Limpa o campo antes de enviar.
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

            // Também não retorna o hash da senha para um único usuário.
            usuario.SenhaHash = string.Empty;

            return usuario;
        }

        // POST: api/Usuarios
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(CreateUsuarioDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var usuario = new Usuario
            {
                Nome = dto.Nome,
                Email = dto.Email,
                Perfil = dto.Perfil,
                SetorIdSetor = dto.SetorIdSetor,
                DataCriacao = System.DateTime.UtcNow,
                SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha) // Cria o hash da senha
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            usuario.SenhaHash = string.Empty; // Limpa antes de retornar.

            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, usuario);
        }

        // ==================================================================
        // AQUI ESTÁ O MÉTODO PUT CORRIGIDO
        // ==================================================================
        // PUT: api/Usuarios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuarioUpdateRequest)
        {
            if (id != usuarioUpdateRequest.Id)
            {
                return BadRequest("O ID da URL não corresponde ao ID do usuário.");
            }

            // 1. Busca o usuário existente e completo do banco de dados.
            var usuarioDoBanco = await _context.Usuarios.FindAsync(id);

            if (usuarioDoBanco == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            // 2. Atualiza APENAS os campos que queremos permitir a edição.
            //    Ignoramos completamente a senha, mantendo a que já existe no banco.
            usuarioDoBanco.Nome = usuarioUpdateRequest.Nome;
            usuarioDoBanco.Email = usuarioUpdateRequest.Email;
            usuarioDoBanco.Perfil = usuarioUpdateRequest.Perfil;
            usuarioDoBanco.SetorIdSetor = usuarioUpdateRequest.SetorIdSetor;

            // 3. O Entity Framework agora é inteligente o suficiente para saber
            //    exatamente quais campos mudaram e só atualizará eles.
            try
            {
                await _context.SaveChangesAsync(); // Salva as alterações reais no banco.
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Usuarios.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); // Retorna sucesso.
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

            return NoContent();
        }
    }
}
