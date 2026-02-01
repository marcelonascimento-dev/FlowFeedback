using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FlowFeedback.Application.Interfaces;
using FlowFeedback.Domain.Entities;
using FlowFeedback.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FlowFeedback.Application.Services;

public sealed class AuthService(
    IUserTenantRepository userTenantRepository,
    IUserRepository userRepository,
    IConfiguration config) : IAuthService
{
    public async Task<string?> AutenticarAsync(string email, string senha)
    {
        // 1. Find UserTenant by Email (checks if user exists and is linked to a tenant)
        // Note: usage of GetByEmailAsync implies 1:1 mapping or default tenant. 
        var userTenant = await userTenantRepository.GetByEmailAsync(email);

        if (userTenant is null || !userTenant.IsActive)
            return null;

        // 2. Find User
        var user = await userRepository.GetByIdAsync(userTenant.UserId);

        if (user is null || !user.IsActive)
            return null;

        // 3. Verify Password
        if (!BCrypt.Net.BCrypt.Verify(senha, user.PasswordHash))
            return null;

        // 4. Generate Token
        return GerarJwtToken(user, userTenant);
    }

    private string GerarJwtToken(User user, UserTenant userTenant)
    {
        var keyString = config["Jwt:Key"]
            ?? throw new InvalidOperationException("Jwt:Key não configurada.");

        var key = Encoding.ASCII.GetBytes(keyString);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, userTenant.Role.ToString()), // Use Enum.ToString()
            new("TenantId", userTenant.TenantId.ToString()) // TenantId Guid
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
