namespace FlowFeedback.Application.DTOs;

public record RegistrarVotoDto(Guid AlvoAvaliacaoId, int Nota, DateTime DataHora, string? TagMotivo);