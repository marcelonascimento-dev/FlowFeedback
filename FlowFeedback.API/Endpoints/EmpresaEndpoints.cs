using FlowFeedback.Application.DTOs;
using FlowFeedback.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FlowFeedback.API.Endpoints;

public static class EmpresaEndpoints
{
    public static void MapEmpresaEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/empresas")
            .WithTags("Empresas")
            .RequireAuthorization();

        group.MapGet("/", async (IEmpresaService service) =>
        {
            var empresas = await service.GetAllAsync();
            return Results.Ok(empresas);
        });

        group.MapGet("/{id:guid}", async (Guid id, IEmpresaService service) =>
        {
            var empresa = await service.GetByIdAsync(id);
            return empresa is not null ? Results.Ok(empresa) : Results.NotFound();
        });

        group.MapPost("/", async (EmpresaCreateDto dto, IEmpresaService service) =>
        {
            var result = await service.AddAsync(dto);
            return Results.Created($"/api/empresas/{result.Id}", result);
        });

        group.MapPut("/{id:guid}", async (Guid id, EmpresaUpdateDto dto, IEmpresaService service) =>
        {
            if (id != dto.Id) return Results.BadRequest("ID divergente.");

            try
            {
                await service.UpdateAsync(dto);
                return Results.NoContent();
            }
            catch (KeyNotFoundException)
            {
                return Results.NotFound();
            }
        });

        group.MapDelete("/{id:guid}", async (Guid id, IEmpresaService service) =>
        {
            await service.DeleteAsync(id);
            return Results.NoContent();
        });
    }
}
