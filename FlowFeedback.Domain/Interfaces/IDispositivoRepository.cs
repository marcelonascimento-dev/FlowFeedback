using FlowFeedback.Domain.Entities;

namespace FlowFeedback.Domain.Interfaces;

public interface IDispositivoRepository
{
    Task<Dispositivo?> GetByIdentifierAsync(string deviceIdentifier);
}