using FlowFeedback.Application.DTOs;
using FlowFeedback.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FlowFeedback.API.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/admin/users")
                       .RequireAuthorization();

        group.MapGet("/", async (IUserService service) =>
            Results.Ok(await service.GetAllAsync()));

        group.MapGet("/{id:guid}", async (Guid id, IUserService service) =>
        {
            var user = await service.GetByIdAsync(id);
            return user is null ? Results.NotFound() : Results.Ok(user);
        });

        group.MapPost("/", async ([FromBody] UserRequest req, IUserService service) =>
        {
            var user = await service.CreateAsync(req);
            return Results.Created($"/api/admin/users/{user.Id}", user);
        });

        group.MapPut("/{id:guid}", async (Guid id, [FromBody] UserUpdateRequest req, IUserService service) =>
        {
            await service.UpdateAsync(id, req);
            return Results.NoContent();
        });

        group.MapDelete("/{id:guid}", async (Guid id, IUserService service) =>
        {
            await service.DeleteAsync(id);
            return Results.NoContent();
        });
    }
}
