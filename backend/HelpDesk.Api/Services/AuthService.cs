// Services/AuthService.cs
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HelpDesk.Api.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace HelpDesk.Api.Services
{
    public class AuthService
    {
        private readonly IConfiguration _configuration;
        private readonly string _jwtKey;
        private readonly string _jwtIssuer;
        private readonly string _jwtAudience;
        private readonly int _expiryHours;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            _jwtKey = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key não configurado.");
            _jwtIssuer = _configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer não configurado.");
            _jwtAudience = _configuration["Jwt:Audience"] ?? throw new InvalidOperationException("Jwt:Audience não configurado.");

            if (!int.TryParse(_configuration["Jwt:ExpiryHours"], out _expiryHours))
            {
                _expiryHours = 2; // default
            }
        }

        /// <summary>
        /// Verifica a senha em texto puro contra o hash armazenado (BCrypt).
        /// </summary>
        public bool VerifyPassword(string plainPassword, string hashedPassword)
        {
            if (string.IsNullOrEmpty(plainPassword) || string.IsNullOrEmpty(hashedPassword))
                return false;

            try
            {
                return BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);
            }
            catch
            {
                // Em caso de qualquer erro no BCrypt, falhar na verificação.
                return false;
            }
        }

        /// <summary>
        /// Gera um token JWT para o usuário fornecido.
        /// Inclui claims: NameIdentifier (id), Email e Role.
        /// </summary>
        public string GenerateJwtToken(Usuario usuario)
        {
            if (usuario == null)
                throw new ArgumentNullException(nameof(usuario));

            var keyBytes = Encoding.UTF8.GetBytes(_jwtKey);
            var key = new SymmetricSecurityKey(keyBytes);
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email ?? string.Empty),
                // armazenamos o perfil exatamente como está no banco, o consumidor (frontend) já
                // normaliza (ex.: .ToLower()) quando necessário -- manteremos o valor original.
                new Claim(ClaimTypes.Role, usuario.Perfil ?? "Usuario")
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = new JwtSecurityToken(
                issuer: _jwtIssuer,
                audience: _jwtAudience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddHours(_expiryHours),
                signingCredentials: creds
            );

            return tokenHandler.WriteToken(token);
        }
    }
}
