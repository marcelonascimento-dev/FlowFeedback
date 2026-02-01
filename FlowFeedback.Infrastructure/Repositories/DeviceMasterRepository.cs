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
            @"SELECT TenantId, HardwareSignature 
              FROM DispositivoKeys 
              WHERE ApiKeyHash = @Key AND Ativo = 1",
            new { Key = apiKeyHash });
    }

    public async Task RegistrarNovoDispositivoAsync(Guid tenantId, string nomeDispositivo, string keyHash)
    {
        using var conn = dbFactory.CreateMasterConnection();

        var sql = @"
            INSERT INTO DispositivoKeys (ApiKeyHash, TenantId, NomeDispositivo, Ativo)
            VALUES (@Hash, @TenantId, @Nome, 1)";

        await conn.ExecuteAsync(sql, new
        {
            Hash = keyHash,
            TenantId = tenantId,
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
