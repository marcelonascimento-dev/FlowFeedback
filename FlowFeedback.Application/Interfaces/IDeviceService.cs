namespace FlowFeedback.Application.Interfaces
{
    public interface IDeviceService
    {
        Task<string> RegistrarNovoDispositivoAsync(int tenantCode, string nomeDispositivo);
    }
}
