using System.Security.Claims;
using System.Text;
using FlowFeedback.Application.DTOs;
using FlowFeedback.Application.Interfaces;
using FlowFeedback.Domain.Entities;
using FlowFeedback.Domain.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace FlowFeedback.Application.Services;

public sealed class AuthService(
    IUserTenantRepository userTenantRepository,
    IOptions<JwtSettings> jwtOptions) : IAuthService
{
    private readonly JwtSettings _settings = jwtOptions.Value;

    public async Task<LoginResponse?> AutenticarAsync(string email, string senha)
    {
        var userTenant = await userTenantRepository.GetByEmailAsync(email);

        if (userTenant?.User is null || userTenant.Tenant is null)
            return null;

        if (!userTenant.IsActive || !userTenant.User.IsActive)
            return null;

        if (!BCrypt.Net.BCrypt.Verify(senha, userTenant.User.PasswordHash))
            return null;

        var token = GerarJwtToken(userTenant.User, userTenant);

        var expiresInSeconds = (int)TimeSpan.FromHours(_settings.ExpirationHours).TotalSeconds;

        return new LoginResponse(
            AccessToken: token,
            ExpiresIn: expiresInSeconds,
            User: new UserResponse(userTenant.User.Id, userTenant.User.Name, userTenant.User.Email, userTenant.Role.ToString()),
            Tenant: new TenantResponse(userTenant.TenantId, userTenant.Tenant.Slug)
        );
    }

    private string GerarJwtToken(User user, UserTenant userTenant)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new("tenant_id", userTenant.TenantId.ToString()),
                new(ClaimTypes.Role, userTenant.Role.ToString())
            ]),
            Expires = DateTime.UtcNow.AddHours(_settings.ExpirationHours),
            SigningCredentials = credentials,
            Issuer = _settings.Issuer,
            Audience = _settings.Audience
        };

        var handler = new JsonWebTokenHandler();
        return handler.CreateToken(tokenDescriptor);
    }

    public string HashSenha(string senha)
        => BCrypt.Net.BCrypt.HashPassword(senha);
}

public record LoginResponse(
    string AccessToken,
    int ExpiresIn,
    UserResponse User,
    TenantResponse Tenant,
    string TokenType = "Bearer");

public record UserResponse(Guid Id, string Name, string Email, string Role);
public record TenantResponse(Guid Id, string Slug);