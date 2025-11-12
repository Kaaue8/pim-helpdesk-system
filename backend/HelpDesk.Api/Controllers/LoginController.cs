using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HelpDesk.Api.Data;
using HelpDesk.Api.Models;
using HelpDesk.Api.Models.Dto;
using HelpDesk.Api.Services;
using System.Threading.Tasks;
using BCrypt.Net;
using System.ComponentModel.DataAnnotations;

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

        // DTO para Login (apenas Email e Senha)
        public class LoginDto
        {
            [Required, EmailAddress]
            public required string Email { get; set; }

            [Required]
            public required string Senha { get; set; }
        }

        // POST: api/Login/Register
        // Usa o CreateUsuarioDto para criar um novo usuário
        [HttpPost("Register")]
        public async Task<ActionResult<Usuario>> Register(CreateUsuarioDto dto)
        {
            // 1. Validação de Email Único
            if (await _context.Usuarios.AnyAsync(u => u.Email == dto.Email))
            {
                return BadRequest("O email fornecido já está em uso.");
            }

            // 2. Criação do objeto Usuario a partir do DTO
            var usuario = new Usuario
            {
                Nome = dto.Nome,
                Email = dto.Email,
                Perfil = dto.Perfil,
                SetorIdSetor = dto.SetorIdSetor,
                DataCriacao = System.DateTime.UtcNow,
                SenhaHash = string.Empty // Inicializa o membro requerido
            };

            // 3. Hashing da Senha (LGPD Compliance)
            usuario.SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha);

            // 4. Adiciona o usuário ao contexto e salva no banco de dados
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            // 5. Retorna o usuário criado (sem o hash)
            usuario.SenhaHash = string.Empty;

            return CreatedAtAction(nameof(Register), new { id = usuario.Id }, usuario);
        }

        // POST: api/Login/Authenticate
        // Autentica o usuário e retorna o JWT
        [HttpPost("Authenticate")]
        public async Task<ActionResult<object>> Authenticate(LoginDto dto)
        {
            // 1. Busca o usuário pelo email
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (usuario == null)
            {
                return Unauthorized(new { message = "Email ou senha inválidos." });
            }

            // 2. Verifica a senha usando o AuthService (BCrypt)
            if (!_authService.VerifyPassword(dto.Senha, usuario.SenhaHash))
            {
                return Unauthorized(new { message = "Email ou senha inválidos." });
            }

            // 3. Gera o Token JWT
            var token = _authService.GenerateJwtToken(usuario);

            // 4. Retorna o Token e informações básicas do usuário
            return Ok(new
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