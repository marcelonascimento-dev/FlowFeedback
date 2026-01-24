using FlowFeedback.Application.DTOs;

namespace FlowFeedback.Application.Events;

public record PacoteVotosRecebidoEvent
{
    public PacoteVotosDto Dados { get; init; } = default!;
    public DateTime DataIngestao { get; init; } = DateTime.UtcNow;
}