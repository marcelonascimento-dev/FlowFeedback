namespace FlowFeedback.Device.Models;

public record VotoItemDto(
    Guid AlvoAvaliacaoId,
    int Nota,
    DateTime DataHora,
    string TagMotivo = "");
