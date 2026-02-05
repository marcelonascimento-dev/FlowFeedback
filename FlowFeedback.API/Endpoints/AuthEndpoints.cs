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
            var token = await service.AutenticarAsync(req.Email, req.Senha);
            return token is null ? Results.Unauthorized() : Results.Ok(new { Token = token });
        });

        var adminGroup = app.MapGroup("/api/admin/devices")
                            .RequireAuthorization();

        adminGroup.MapPost("/generate-key", async (
            [FromBody] CreateDeviceRequest req,
            IDeviceService deviceService,
            HttpContext http) =>
        {
            var tenantClaim = http.User.FindFirst("TenantId")?.Value;

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