using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FlowFeedback.Application.Interfaces;
using FlowFeedback.Domain.Entities;
using FlowFeedback.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FlowFeedback.Application.Services;

public sealed class AuthService : IAuthService
{
    private readonly ITenantUserIndexRepository _tenantUserIndexRepository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IConfiguration _config;

    public AuthService(
        ITenantUserIndexRepository tenantUserIndexRepository,
        IUsuarioRepository usuarioRepository,
        IConfiguration config)
    {
        _tenantUserIndexRepository = tenantUserIndexRepository;
        _usuarioRepository = usuarioRepository;
        _config = config;
    }

    public async Task<string?> AutenticarAsync(string email, string senha)
    {
        var index = await _tenantUserIndexRepository.GetByEmailAsync(email);

        if (index is null || !index.Ativo)
            return null;

        var user = await _usuarioRepository.ObterPorIdAsync(index.UserId);

        if (user is null || !user.Ativo)
            return null;

        if (!BCrypt.Net.BCrypt.Verify(senha, user.SenhaHash))
            return null;

        // 4️⃣ Gerar JWT com TenantCode
        return GerarJwtToken(user, index.TenantCodigo);
    }

    private string GerarJwtToken(Usuario user, long tenantCodigo)
    {
        var keyString = _config["Jwt:Key"]
            ?? throw new InvalidOperationException("Jwt:Key não configurada.");

        var key = Encoding.ASCII.GetBytes(keyString);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim("TenantCode", tenantCodigo.ToString())
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

    public string HashSenha(string senha)
        => BCrypt.Net.BCrypt.HashPassword(senha);
}
