using FlowFeedback.Application.DTOs;
using FlowFeedback.Application.Interfaces;

namespace FlowFeedback.API.Endpoints;

public static class CadastroEndpoints
{
    public static void MapCadastroEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/cadastro")
            .WithTags("Cadastro");

        group.MapPost("/tenant", async (ICadastroService service, CreateTenantDto dto) =>
        {
            var result = await service.CadastrarTenantAsync(dto);
            return Results.Created($"/api/cadastro/tenant/{result.Id}", result);
        })
        .Produces<TenantSaidaDto>(StatusCodes.Status201Created);

        group.MapPost("/unidade", async (ICadastroService service, CreateUnidadeDto dto) =>
        {
            try
            {
                var result = await service.CadastrarUnidadeAsync(dto);
                return Results.Created($"/api/cadastro/unidade/{result.Id}", result);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { erro = ex.Message });
            }
        })
        .Produces<UnidadeSaidaDto>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest);

        group.MapPost("/dispositivo", async (ICadastroService service, CreateDispositivoDto dto) =>
        {
            try
            {
                var result = await service.CadastrarDispositivoAsync(dto);
                return Results.Created($"/api/cadastro/dispositivo/{result.Id}", result);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { erro = ex.Message });
            }
        })
        .Produces<DispositivoSaidaDto>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest);

        group.MapPost("/alvo-avaliacao", async (ICadastroService service, CreateAlvoAvaliacaoDto dto) =>
        {
            try
            {
                var result = await service.CadastrarAlvoAsync(dto);
                return Results.Created($"/api/cadastro/alvo-avaliacao/{result.Id}", result);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { erro = ex.Message });
            }
        })
        .Produces<AlvoAvaliacaoSaidaDto>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest);

        group.MapPost("/alvo-dispositivo", async (ICadastroService service, CreateAlvoDispositivoDto dto) =>
        {
            try
            {
                await service.VincularAlvosDispositivoAsync(dto);
                return Results.Created($"/api/cadastro/dispositivo/{dto.DispositivoId}", null);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { erro = ex.Message });
            }
        })
        .Produces(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest);
    }
}