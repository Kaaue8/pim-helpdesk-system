using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HelpDesk.Api.Data;
using HelpDesk.Api.Models;
using HelpDesk.Api.Services;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly AuthService _authService;

        public LoginController(ApplicationDbContext context, AuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        // DTO para Login
        public class LoginDto
        {
            [Required, EmailAddress]
            public required string Email { get; set; }

            [Required]
            public required string Senha { get; set; }
        }

        // POST: api/Login/Authenticate
        // Autentica o usuário e retorna o JWT
        [HttpPost("Authenticate")]
        public async Task<ActionResult<object>> Authenticate(LoginDto dto)
        {
            // 1. Busca o usuário pelo email
            var usuario = await _context.Usuarios
                .Include(u => u.Setor)
                .FirstOrDefaultAsync(u => u.Email == dto.Email && u.Ativo);

            if (usuario == null)
            {
                return Unauthorized(new { message = "Email ou senha inválidos." });
            }

            // 2. ✅ Verifica a senha usando BCrypt com SenhaHash
            if (!_authService.VerifyPassword(dto.Senha, usuario.SenhaHash))
            {
                return Unauthorized(new { message = "Email ou senha inválidos." });
            }

            // 3. Gera o Token JWT
            var token = _authService.GenerateJwtToken(usuario);

            // 4. Retorna o Token e informações do usuário
            return Ok(new
            {
                Token = token,
                Usuario = new
                {
                    IdUsuario = usuario.IdUsuario,
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    Perfil = usuario.Perfil,
                    SetorId = usuario.SetorId,
                    SetorNome = usuario.Setor?.Nome,
                    Telefone = usuario.Telefone,
                    Ativo = usuario.Ativo
                }
            });
        }
    }
}

