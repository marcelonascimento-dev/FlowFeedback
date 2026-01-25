using Dapper;
using FlowFeedback.Domain.Interfaces;
using FlowFeedback.Domain.Models;
using FlowFeedback.Infrastructure.Data;

namespace FlowFeedback.Infrastructure.Repositories;

public class DeviceMasterRepository(IDbConnectionFactory dbFactory) : IDeviceMasterRepository
{
    public async Task<DeviceLicencaDto?> ObterLicencaPorChaveAsync(string apiKeyHash)
    {
        using var conn = dbFactory.CreateMasterConnection();

        return await conn.QueryFirstOrDefaultAsync<DeviceLicencaDto>(
            @"SELECT TenantCode, HardwareSignature 
              FROM DispositivoKeys 
              WHERE ApiKeyHash = @Key AND Ativo = 1",
            new { Key = apiKeyHash });
    }

    public async Task RegistrarNovoDispositivoAsync(int tenantCode, string nomeDispositivo, string keyHash)
    {
        using var conn = dbFactory.CreateMasterConnection();

        var sql = @"
            INSERT INTO DispositivoKeys (ApiKeyHash, TenantCode, NomeDispositivo, Ativo)
            VALUES (@Hash, @Tenant, @Nome, 1)";

        await conn.ExecuteAsync(sql, new
        {
            Hash = keyHash,
            Tenant = tenantCode,
            Nome = nomeDispositivo
        });
    }

    public async Task VincularHardwareAsync(string apiKeyHash, string hardwareSignature)
    {
        using var conn = dbFactory.CreateMasterConnection();

        await conn.ExecuteAsync(
            @"UPDATE DispositivoKeys 
              SET HardwareSignature = @Sig 
              WHERE ApiKeyHash = @Key",
            new { Key = apiKeyHash, Sig = hardwareSignature });
    }
}
