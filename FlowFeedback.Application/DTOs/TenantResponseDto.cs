using FlowFeedback.Domain.Enums;

namespace FlowFeedback.Application.Models;

public sealed class TenantResponseDto
{
    public Guid Id { get; init; }

    public long Codigo { get; init; }

    public string Nome { get; init; } = string.Empty;

    public string Slug { get; init; } = string.Empty;

    public EnumStatusCadastro Status { get; init; }

    public EnumTipoAmbiente TipoAmbiente { get; init; }

    public DateTime DataCriacao { get; init; }
}
