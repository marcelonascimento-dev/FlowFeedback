namespace FlowFeedback.Application.DTOs;

public record PacoteVotosDto(
    Guid TenantId,          
    Guid DeviceId,          
    List<RegistrarVotoDto> Votos 
);