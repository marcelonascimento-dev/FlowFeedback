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

    }
}
