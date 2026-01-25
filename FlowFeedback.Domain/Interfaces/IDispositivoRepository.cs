using FlowFeedback.Domain.Entities;

namespace FlowFeedback.Domain.Interfaces;

public interface IDispositivoRepository
{
    Task<Dispositivo?> GetByIdentifierAsync(string deviceIdentifier);
    Task<Dispositivo?> GetDispositivoWithAlvosAsync(Guid id);
    Task UpdateDispositivoAsync(Dispositivo dispositivo);
    Task<bool> DispositivoExisteAsync(string identificador);
    Task<Dispositivo> AddDispositivoAsync(Dispositivo dispositivo);
}