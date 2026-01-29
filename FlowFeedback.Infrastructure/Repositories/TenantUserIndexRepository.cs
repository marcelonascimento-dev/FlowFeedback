using Dapper;
using FlowFeedback.Domain.Entities;
using FlowFeedback.Domain.Interfaces;
using FlowFeedback.Infrastructure.Data;

namespace FlowFeedback.Infrastructure.Repositories;

public sealed class TenantUserIndexRepository(IDbConnectionFactory connectionFactory) : ITenantUserIndexRepository
{
    public async Task<TenantUserIndex?> GetByEmailAsync(string email)
    {
        const string sql = @"
            SELECT 
                UserId,
                TenantCodigo,
                Ativo
            FROM TenantUserIndex
            WHERE Email = @Email";

        using var db = connectionFactory.CreateMasterConnection();

        return await db.QueryFirstOrDefaultAsync<TenantUserIndex>(
            sql,
            new { Email = email }
        );
    }
}
