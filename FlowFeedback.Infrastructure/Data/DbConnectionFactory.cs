using System.Data;
using Dapper;
using FlowFeedback.Domain.Entities;
using FlowFeedback.Infrastructure.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;

namespace FlowFeedback.Infrastructure.Data;

public class DbConnectionFactory : IDbConnectionFactory
{
    private readonly IConfiguration _config;
    private readonly IHttpContextAccessor _httpContext;
    private readonly IDistributedCache _cache;
    private readonly string _masterConnection;
    private readonly ICryptoService _crypto;

    public DbConnectionFactory(IConfiguration config, IHttpContextAccessor httpContext, IDistributedCache cache, ICryptoService crypto)
    {
        _config = config;
        _httpContext = httpContext;
        _cache = cache;
        _masterConnection = _config.GetConnectionString("DefaultConnection");
        _crypto = crypto;
    }

    public IDbConnection CreateMasterConnection() => new SqlConnection(_masterConnection);

    public IDbConnection CreateTenantConnection()
    {
        var tenantCodeStr = _httpContext.HttpContext?.Request.Headers["X-Tenant-Code"].ToString();
        if (!int.TryParse(tenantCodeStr, out int tenantCode))
            throw new UnauthorizedAccessException("Tenant inválido.");

        string cacheKey = $"tenant_conn_{tenantCode}";
        string connectionString = _cache.GetString(cacheKey);

        if (string.IsNullOrEmpty(connectionString))
        {
            connectionString = ResolveConnectionStringFromMaster(tenantCode);

            _cache.SetString(cacheKey, connectionString, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            });
        }

        return new SqlConnection(connectionString);
    }

    private string ResolveConnectionStringFromMaster(int tenantCode)
    {
        using var masterDb = CreateMasterConnection();
        var meta = masterDb.QueryFirstOrDefault<TenantMetadata>(
            "SELECT DbServer, DbName, DbUser, DbPasswordEncrypted FROM Tenants WHERE Id = @Id",
            new { Id = tenantCode });

        if (meta == null) throw new Exception("Configuração de banco não encontrada para este Tenant.");

        var builder = new SqlConnectionStringBuilder(_masterConnection)
        {
            DataSource = meta.DbServer,
            InitialCatalog = meta.DbName,
            TrustServerCertificate = true
        };

        if (!string.IsNullOrEmpty(meta.DbUser))
        {
            builder.UserID = meta.DbUser;
            builder.Password = _crypto.Decrypt(meta.DbPasswordEncrypted);
        }

        return builder.ConnectionString;
    }
}