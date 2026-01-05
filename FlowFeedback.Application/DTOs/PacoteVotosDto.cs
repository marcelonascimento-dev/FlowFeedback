namespace FlowFeedback.Application.DTOs;

public record PacoteVotosDto(
    Guid TenantId,          
    string DeviceId,          
    List<RegistrarVotoDto> Votos 
);