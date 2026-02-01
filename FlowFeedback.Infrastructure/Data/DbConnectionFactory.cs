using System.Data;
using System.Text;
using Dapper;
using FlowFeedback.Application.Interfaces;
using FlowFeedback.Application.Interfaces.Security;
using FlowFeedback.Infrastructure.Data;
using FlowFeedback.Infrastructure.Security;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;

namespace FlowFeedback.Infrastructure.Data;

public sealed class DbConnectionFactory(
    IConfiguration configuration,
    IDistributedCache cache,
    ITenantContext tenant,
    ICryptoService cryptoService) : IDbConnectionFactory
{
    private const string TenantCacheKeyPrefix = "tenant:connection:";
    private static readonly TimeSpan TenantCacheTtl = TimeSpan.FromMinutes(10);

    private readonly string _masterConnectionString =
            configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("DefaultConnection não configurada.");
    public IDbConnection CreateMasterConnection() => new SqlConnection(_masterConnectionString);

    public IDbConnection CreateTenantConnection()
    {
        // Now resolves using TenantId from ITenantContext, assuming it provides the ID.
        // We might need to update ITenantContext to provide Guid TenantId instead of int Code.
        // Assuming ITenantContext is updated later. For now let's assume valid TenantId in context.
        var cacheKey = $"{TenantCacheKeyPrefix}{tenant.TenantId}";
        var connectionString = cache.GetString(cacheKey);

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            connectionString = ResolveConnectionStringFromMaster(tenant.TenantId);

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

    private string ResolveConnectionStringFromMaster(Guid tenantId)
    {
        const string sql = """
            SELECT DbServer, DbName, DbUser, DbPassword
            FROM Tenants
            WHERE Id = @Id AND Status = 1
        """;

        using var connection = CreateMasterConnection();

        var tenantInfo = connection.QueryFirstOrDefault(sql, new { Id = tenantId });

        if (tenantInfo == null)
            throw new UnauthorizedAccessException("Tenant inválido ou inativo.");

        // Construct connection string
        var builder = new SqlConnectionStringBuilder
        {
            DataSource = tenantInfo.DbServer,
            InitialCatalog = tenantInfo.DbName,
            UserID = tenantInfo.DbUser,
            Password = cryptoService.Decrypt(Encoding.UTF8.GetString((byte[])tenantInfo.DbPassword)),
            TrustServerCertificate = true
        };

        return builder.ConnectionString;
    }
}
