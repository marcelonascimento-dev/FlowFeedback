using System.Data;
using Dapper;
using FlowFeedback.Application.Interfaces;
using FlowFeedback.Application.Interfaces.Security;
using FlowFeedback.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;

public sealed class DbConnectionFactory(
    IConfiguration configuration,
    IDistributedCache cache,
    ISecretProvider secretProvider,
    ITenantContext tenant) : IDbConnectionFactory
{
    private const string TenantCacheKeyPrefix = "tenant:connection:";
    private static readonly TimeSpan TenantCacheTtl = TimeSpan.FromMinutes(10);

    private readonly string _masterConnectionString =
            configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("DefaultConnection não configurada.");
    public IDbConnection CreateMasterConnection() => new SqlConnection(_masterConnectionString);

    public IDbConnection CreateTenantConnection()
    {
        var cacheKey = $"{TenantCacheKeyPrefix}{tenant.TenantCode}";
        var connectionString = cache.GetString(cacheKey);

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            connectionString = ResolveConnectionStringFromMaster(tenant.TenantCode);

            cache.SetString(
                cacheKey,
                connectionString,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TenantCacheTtl
                });
        }

        return new SqlConnection(connectionString);
    }

    private string ResolveConnectionStringFromMaster(int tenantCode)
    {
        const string sql = """
            SELECT ConnectionSecretKey
            FROM Tenants
            WHERE Codigo = @Codigo AND Status = 1
        """;

        using var connection = CreateMasterConnection();

        var secretKey = connection.QueryFirstOrDefault<string>(sql, new { Codigo = tenantCode });

        if (string.IsNullOrWhiteSpace(secretKey))
            throw new UnauthorizedAccessException("Tenant inválido ou inativo.");

        return secretProvider
            .GetSecretAsync(secretKey)
            .GetAwaiter()
            .GetResult()
            ?? throw new InvalidOperationException("Segredo não encontrado no vault.");
    }
}
