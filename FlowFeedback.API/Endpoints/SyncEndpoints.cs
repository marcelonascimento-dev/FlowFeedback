using FlowFeedback.Application.DTOs;
using FlowFeedback.Application.Events;
using FlowFeedback.Application.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace FlowFeedback.API.Endpoints;

public static class SyncEndpoints
{
    public static void MapSyncEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/sync")
            .WithTags("Sync");

        group.MapPost("/votos", async (
            [FromServices] IConfiguration config,
            [FromServices] IServiceProvider serviceProvider,
            [FromServices] IFeedbackService feedbackService,
            [FromBody] PacoteVotosDto pacote) =>
        {
            if (pacote == null || pacote.Votos.Count == 0)
                return Results.BadRequest("Pacote inválido ou vazio.");

            var useQueue = config.GetValue<bool>("Messaging:UseQueue");

            if (useQueue)
            {
                var publishEndpoint = serviceProvider.GetRequiredService<IPublishEndpoint>();

                var evento = new PacoteVotosRecebidoEvent
                {
                    Dados = pacote
                };

                await publishEndpoint.Publish(evento);
                return Results.Accepted();
            }
            else
            {
                // Execução direta
                await feedbackService.ProcessarPacoteVotos(pacote);
                return Results.Ok(new { Message = "Processado diretamente com sucesso." });
            }
        })
        .Produces(StatusCodes.Status202Accepted)
        .Produces(StatusCodes.Status400BadRequest);
    }
}