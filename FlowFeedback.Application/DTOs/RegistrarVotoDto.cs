namespace FlowFeedback.Application.DTOs;

public record RegistrarVotoDto(Guid Id, Guid IdAlvoAvaliacao, int Valor, string? Comentario, IEnumerable<string>? Tags, DateTime DataHora);
