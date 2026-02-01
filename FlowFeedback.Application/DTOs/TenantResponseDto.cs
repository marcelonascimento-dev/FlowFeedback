using FlowFeedback.Domain.Enums;

namespace FlowFeedback.Application.Models;

public sealed class TenantResponseDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string Slug { get; init; } = string.Empty;

    public EnumStatusCadastro Status { get; init; }

    public DateTime CreatedAt { get; init; }
}
