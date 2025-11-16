using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using HelpDesk.Api.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace HelpDesk.Api.Services
{
    public class AuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // ✅ Método para verificar a senha com BCrypt
        public bool VerifyPassword(string password, string senhaHash)
        {
            try
            {
                // Validação com BCrypt (seguro)
                return BCrypt.Net.BCrypt.Verify(password, senhaHash);
            }
            catch (Exception)
            {
                // Se falhar (senha inválida ou hash corrompido)
                return false;
            }
        }

        // ✅ Método para gerar hash de senha com BCrypt
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        // ✅ Método para gerar o JWT
        public string GenerateJwtToken(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var jwtKey = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(jwtKey))
            {
                throw new InvalidOperationException("Jwt:Key não configurado em appsettings.json");
            }
            var key = Encoding.ASCII.GetBytes(jwtKey);

            // Claims (informações do usuário que serão incluídas no token)
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Name, usuario.Nome),
                new Claim(ClaimTypes.Role, usuario.Perfil) // Admin ou Usuario
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(8), // Token expira em 8 horas
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}

