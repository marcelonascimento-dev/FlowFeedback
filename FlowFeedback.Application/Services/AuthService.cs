using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FlowFeedback.Application.Interfaces;
using FlowFeedback.Domain.Entities;
using FlowFeedback.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FlowFeedback.Application.Services;

public class AuthService(IUsuarioRepository usuarioRepository,IConfiguration config) : IAuthService
{
    public async Task<string?> AutenticarAsync(string email, string senha)
    {
        var user = await usuarioRepository.ObterPorEmailAsync(email);

        if (user == null) return null;

        if (!BCrypt.Net.BCrypt.Verify(senha, user.SenhaHash)) return null;

        return GerarJwtToken(user);
    }

    private string GerarJwtToken(Usuario user)
    {
        var keyString = config["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key não configurada.");
        var key = Encoding.ASCII.GetBytes(keyString);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim("TenantCode", user.TenantCode.ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(8),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public string HashSenha(string senha) => BCrypt.Net.BCrypt.HashPassword(senha);
}