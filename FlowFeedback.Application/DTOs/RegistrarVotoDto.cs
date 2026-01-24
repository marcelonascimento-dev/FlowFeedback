namespace FlowFeedback.Application.DTOs;

public record RegistrarVotoDto(Guid Id, Guid IdAlvoAvaliacao, int Valor, string? Comentario, List<string>? Tags, DateTime DataHora);
