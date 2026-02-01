namespace FlowFeedback.Application.Interfaces
{
    public interface IDeviceService
    {
        Task<string> RegistrarNovoDispositivoAsync(Guid tenantId, string nomeDispositivo);
    }
}
