using FlowFeedback.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FlowFeedback.API.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var publicGroup = app.MapGroup("/api/auth");

        publicGroup.MapPost("/login", async (LoginRequest req, IAuthService service) =>
        {
            var response = await service.AutenticarAsync(req.Email, req.Senha);
            return response is null ? Results.Unauthorized() : Results.Ok(response);
        });

        publicGroup.MapGet("/me", (HttpContext http) =>
        {
            var user = http.User;
            return Results.Ok(new
            {
                UserId = user.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value,
                Email = user.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Email)?.Value,
                TenantId = user.FindFirst("tenant_id")?.Value,
                Role = user.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value
            });
        }).RequireAuthorization();

        var adminGroup = app.MapGroup("/api/admin/devices")
                            .RequireAuthorization();

        adminGroup.MapPost("/generate-key", async (
            [FromBody] CreateDeviceRequest req,
            IDeviceService deviceService,
            HttpContext http) =>
        {
            var tenantClaim = http.User.FindFirst("tenant_id")?.Value;

            if (!Guid.TryParse(tenantClaim, out Guid tenantId))
                return Results.Forbid();

            var apiKey = await deviceService.RegistrarNovoDispositivoAsync(tenantId, req.Nome);

            return Results.Ok(new
            {
                Message = "Dispositivo registrado com sucesso.",
                DeviceKey = apiKey,
                Instrucao = "Salve esta chave no arquivo de configuração do Quiosque."
            });
        });
    }
}

public record LoginRequest(string Email, string Senha);
public record CreateDeviceRequest(string Nome);