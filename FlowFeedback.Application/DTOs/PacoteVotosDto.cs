namespace FlowFeedback.Application.DTOs;

public record PacoteVotosDto(
    string DeviceId,
    List<RegistrarVotoDto> Votos
);