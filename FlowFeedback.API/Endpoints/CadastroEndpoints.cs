using FlowFeedback.Application.DTOs;
using FlowFeedback.Application.Interfaces;

namespace FlowFeedback.API.Endpoints;

public static class CadastroEndpoints
{
    public static void MapCadastroEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/cadastro")
            .WithTags("Cadastro");

    }
}