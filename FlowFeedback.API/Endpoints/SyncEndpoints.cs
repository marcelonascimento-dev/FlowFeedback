using FlowFeedback.Application.DTOs;
using FlowFeedback.Application.Events;
using MassTransit;

namespace FlowFeedback.API.Endpoints;

public static class SyncEndpoints
{
    public static void MapSyncEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/sync")
            .WithTags("Sync");

        group.MapPost("/votos", async (IPublishEndpoint publishEndpoint, PacoteVotosDto pacote) =>
        {
            if (pacote == null || pacote.Votos.Count == 0)
                return Results.BadRequest("Pacote inválido ou vazio.");

            var evento = new PacoteVotosRecebidoEvent
            {
                Dados = pacote
            };

            await publishEndpoint.Publish(evento);

            return Results.Accepted();
        })
        .Produces(StatusCodes.Status202Accepted)
        .Produces(StatusCodes.Status400BadRequest);
    }
}