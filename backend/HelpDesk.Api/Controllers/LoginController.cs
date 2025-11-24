using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HelpDesk.Api.Data;
using HelpDesk.Api.Models;
using HelpDesk.Api.Models.Dto;
using HelpDesk.Api.Services;

namespace HelpDesk.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly AuthService _authService;

        public LoginController(AppDbContext context, AuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        // ============================================================
        // REGISTRO
        // ============================================================
        [HttpPost("Register")]
        public async Task<ActionResult<object>> Register(CreateUsuarioDto dto)
        {
            if (await _context.Usuarios.AnyAsync(u => u.Email == dto.Email))
                return BadRequest(new { success = false, message = "O email fornecido já está em uso." });

            var usuario = new Usuario
            {
                Nome = dto.Nome,
                Email = dto.Email,
                Perfil = dto.Perfil.ToLower(),
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
        // LOGIN
        // ============================================================
        [HttpPost("Authenticate")]
        public async Task<ActionResult<object>> Authenticate(LoginDto dto)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (usuario == null)
                return Unauthorized(new { success = false, message = "Email ou senha inválidos." });

            if (!_authService.VerifyPassword(dto.Senha, usuario.SenhaHash))
                return Unauthorized(new { success = false, message = "Email ou senha inválidos." });

            var token = _authService.GenerateJwtToken(usuario);

            return Ok(new LoginResponseDto
            {
                Token = token,
                UsuarioId = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Perfil = usuario.Perfil
            });
        }
    }
}
