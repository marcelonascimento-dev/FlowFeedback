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
                    Nome,
                    Slug,
                    Status,
                    TipoAmbiente,
                    ConnectionSecretKey
                )
                OUTPUT 
                    inserted.Id,
                    inserted.Codigo,
                    inserted.DataCriacao
                VALUES (
                    @Id,
                    @Nome,
                    @Slug,
                    @Status,
                    @TipoAmbiente,
                    @ConnectionSecretKey
                );";

            using var db = dbConnectionFactory.CreateMasterConnection();

            var result = await db.QuerySingleAsync<Tenant>(sql, tenant);

            tenant.Codigo = result.Codigo;
            tenant.DataCriacao = result.DataCriacao;

            return tenant;

        }

        public async Task<Tenant?> GetTenantAsync(Guid tenantId)
        {
            const string sql = @"
                SELECT 
                    Id,
                    Codigo,
                    Nome,
                    Slug,
                    Status,
                    TipoAmbiente,
                    ConnectionSecretKey,
                    DataCriacao
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
                    Codigo,
                    Nome,
                    Slug,
                    Status,
                    TipoAmbiente,
                    ConnectionSecretKey,
                    DataCriacao
                FROM Tenants
                WHERE Slug = @Slug";

            using var db = dbConnectionFactory.CreateMasterConnection();

            return await db.QueryFirstOrDefaultAsync<Tenant>(
                sql,
                new { Slug = slug }
            );
        }


        public async Task<Tenant?> GetTenantAsync(long codigo)
        {
            const string sql = @"
                SELECT 
                    Id,
                    Codigo,
                    Nome,
                    Slug,
                    Status,
                    TipoAmbiente,
                    ConnectionSecretKey,
                    DataCriacao
                FROM Tenants
                WHERE Codigo = @Codigo";

            using var db = dbConnectionFactory.CreateMasterConnection();

            return await db.QueryFirstOrDefaultAsync<Tenant>(
                sql,
                new { Codigo = codigo }
            );
        }

    }
}
