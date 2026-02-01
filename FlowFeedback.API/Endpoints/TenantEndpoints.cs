using FlowFeedback.Application.DTOs;
using FlowFeedback.Application.Services;
using FlowFeedback.Application.Interfaces;
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
            ITenantService service) =>
        {
            var (tenantId, created) = await service.CadastrarTenantAsync(dto);

            return Results.Created($"/api/admin/tenants/{tenantId}", new
            {
                Id = tenantId,
                CreatedAt = created,
                Message = "Tenant criado com sucesso."
            });
        });

        adminGroup.MapGet("/{id:guid}", async (
            Guid id,
            ITenantService service) =>
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
            ITenantService service) =>
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
    }
}
