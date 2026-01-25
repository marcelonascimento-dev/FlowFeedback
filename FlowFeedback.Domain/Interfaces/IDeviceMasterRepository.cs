using System;
using System.Collections.Generic;
using System.Text;
using FlowFeedback.Domain.Models;

namespace FlowFeedback.Domain.Interfaces
{
    public interface IDeviceMasterRepository
    {
        Task<DeviceLicencaDto?> ObterLicencaPorChaveAsync(string apiKeyHash);
        Task VincularHardwareAsync(string apiKeyHash, string hardwareSignature);
        Task RegistrarNovoDispositivoAsync(int tenantCode, string nomeDispositivo, string keyHash);
    }
}
