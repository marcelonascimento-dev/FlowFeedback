using FlowFeedback.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FlowFeedback.API.Endpoints;

public static class SetupEndpoints
{
    public static void MapSetupEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/setup");

        group.MapPost("/bootstrap", async (
            [FromBody] BootstrapRequest req,
            ISetupService setupService,
            IConfiguration config) =>
        {
            // Security: In a real scenario, check a MasterKey in headers or config
            // For now, let's keep it simple or check if users table is empty (logic is in service)

            var success = await setupService.CreateInitialAdminAsync(req.Email, req.Password);

            if (!success)
                return Results.BadRequest("Não foi possível realizar o bootstrap. Verifique se o usuário já existe.");

            return Results.Ok(new { Message = "Sistema inicializado com sucesso. Você já pode fazer login." });
        });
    }
}

public record BootstrapRequest(string Email, string Password);
