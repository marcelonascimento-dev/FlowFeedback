using Dapper;
using FlowFeedback.Domain.Entities;
using FlowFeedback.Domain.Interfaces;
using FlowFeedback.Infrastructure.Data;

namespace FlowFeedback.Infrastructure.Repositories
{
    public class TenantRepository(IDbConnectionFactory dbConnectionFactory) : ITenantRepository
    {
        public async Task<Tenant> CadastrarTenantAsync(Tenant tenant)
        {
            const string sql = @"
                INSERT INTO Tenants (
                    Id,
                    Name,
                    Slug,
                    Status,
                    DbServer,
                    DbName,
                    DbUser,
                    DbPassword
                )
                OUTPUT 
                    inserted.Id,
                    inserted.CreatedAt
                VALUES (
                    @Id,
                    @Name,
                    @Slug,
                    @Status,
                    @DbServer,
                    @DbName,
                    @DbUser,
                    @DbPassword
                );";

            using var db = dbConnectionFactory.CreateMasterConnection();

            var result = await db.QuerySingleAsync<Tenant>(sql, tenant);

            tenant.CreatedAt = result.CreatedAt;

            return tenant;

        }

        public async Task<Tenant?> GetTenantAsync(Guid tenantId)
        {
            const string sql = @"
                SELECT 
                    Id,
                    Name,
                    Slug,
                    Status,
                    DbServer,
                    DbName,
                    DbUser,
                    DbPassword,
                    CreatedAt
                FROM Tenants
                WHERE Id = @Id";

            using var db = dbConnectionFactory.CreateMasterConnection();

            return await db.QueryFirstOrDefaultAsync<Tenant>(
                sql,
                new { Id = tenantId }
            );
        }


        public async Task<Tenant?> GetTenantAsync(string slug)
        {
            const string sql = @"
                SELECT 
                    Id,
                    Name,
                    Slug,
                    Status,
                    DbServer,
                    DbName,
                    DbUser,
                    DbPassword,
                    CreatedAt
                FROM Tenants
                WHERE Slug = @Slug";

            using var db = dbConnectionFactory.CreateMasterConnection();

            return await db.QueryFirstOrDefaultAsync<Tenant>(
                sql,
                new { Slug = slug }
            );
        }
        public async Task<bool> CriarSchemaTenantAsync(Guid tenantId)
        {
            var schemaName = $"tenant_{tenantId:N}";
            using var masterConn = dbConnectionFactory.CreateMasterConnection();

            // Create the database
            await masterConn.ExecuteAsync($"CREATE DATABASE [{schemaName}]");

            // Connect to the new database to run the schema script
            // This assumes the DbConnectionFactory can handle dynamic connection strings or we build it here.
            // For now, let's stick to the logic from CadastroRepository but improved to target the new DB.

            var builder = new Microsoft.Data.SqlClient.SqlConnectionStringBuilder(masterConn.ConnectionString)
            {
                InitialCatalog = schemaName
            };

            using var tenantConn = new Microsoft.Data.SqlClient.SqlConnection(builder.ConnectionString);
            var scriptTabelas = ScriptProvider.GetTenantSchemaScript();

            await tenantConn.ExecuteAsync(scriptTabelas);
            return true;
        }
    }
}
