// /home/ubuntu/backend/HelpDesk.Api/Services/AuthService.cs

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using HelpDesk.Api.Models;
using Microsoft.Extensions.Configuration;
using BCrypt.Net;
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

        // Método para verificar a senha
        public bool VerifyPassword(string password, string hashedPassword)
        {
            // Usa o BCrypt para verificar se a senha em texto puro corresponde ao hash
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        // Método para gerar o JWT
        public string GenerateJwtToken(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            // CORREÇÃO CS8604: Adicionando verificação de nulidade para Jwt:Key
            var jwtKey = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(jwtKey))
            {
                throw new InvalidOperationException("Jwt:Key não configurado em appsettings.json");
            }
            var key = Encoding.ASCII.GetBytes(jwtKey);

            // Claims (informações do usuário que serão incluídas no token)
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Perfil) // Adiciona o perfil como Role
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(2), // Token expira em 2 horas
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
