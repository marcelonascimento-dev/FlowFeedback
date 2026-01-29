using FlowFeedback.Application.DTOs;
using FlowFeedback.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlowFeedback.API.Endpoints;

public static class TenantEndpoints
{
    public static void MapTenantEndpoints(this IEndpointRouteBuilder app)
    {
        var adminGroup = app
            .MapGroup("/api/admin/tenants")
            .RequireAuthorization();

        adminGroup.MapPost("/", async (
            [FromBody] TenantCreateDto dto,
            TenantService service) =>
        {
            var (tenantId, codigo, dataCriacao) = await service.CadastrarTenantAsync(dto);

            return Results.Created($"/api/admin/tenants/{tenantId}", new
            {
                Id = tenantId,
                Codigo = codigo,
                DataCriacao = dataCriacao,
                Message = "Tenant criado com sucesso."
            });
        });

        adminGroup.MapGet("/{id:guid}", async (
            Guid id,
            TenantService service) =>
        {
            try
            {
                var tenant = await service.GetTenantAsync(id);
                return Results.Ok(tenant);
            }
            catch (KeyNotFoundException)
            {
                return Results.NotFound();
            }
        });

        adminGroup.MapGet("/by-slug/{slug}", async (
            string slug,
            TenantService service) =>
        {
            try
            {
                var tenant = await service.GetTenantAsync(slug);
                return Results.Ok(tenant);
            }
            catch (KeyNotFoundException)
            {
                return Results.NotFound();
            }
        });

        adminGroup.MapGet("/by-code/{codigo:int}", async (
            int codigo,
            TenantService service) =>
        {
            try
            {
                var tenant = await service.GetTenantAsync(codigo);
                return Results.Ok(tenant);
            }
            catch (KeyNotFoundException)
            {
                return Results.NotFound();
            }
        });
    }
}
