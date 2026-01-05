using FlowFeedback.Application.DTOs;

namespace FlowFeedback.Application.Interfaces;

public interface IFeedbackService
{
    Task ProcessarVotosDoTabletAsync(Guid tenantId, string deviceId, List<RegistrarVotoDto> votosDto);
}