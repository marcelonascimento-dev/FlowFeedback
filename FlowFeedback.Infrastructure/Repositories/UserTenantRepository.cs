using Dapper;
using FlowFeedback.Domain.Entities;
using FlowFeedback.Domain.Interfaces;
using FlowFeedback.Infrastructure.Data;

namespace FlowFeedback.Infrastructure.Repositories;

public sealed class UserTenantRepository(IDbConnectionFactory connectionFactory) : IUserTenantRepository
{
    public async Task<UserTenant?> GetByEmailAsync(string email)
    {
        // This query joins Users and UserTenants to get context by email? 
        // Or simply query UserTenants by UserId?
        // The original query was on TenantUserIndex which had Email? 
        // Wait, original TenantUserIndex table had UserId and TenantCode. The repo method `GetByEmailAsync` implies the table had Email or joined. 
        // Original SQL: SELECT UserId, TenantCodigo, Ativo FROM TenantUserIndex WHERE Email = @Email
        // If the new `UserTenants` table doesn't have Email, we need to join `Users`.
        // However, the request didn't specify `Email` in `UserTenants`. 
        // Let's assume we need to join or that the user passes UserId. 
        // But the interface is GetByEmailAsync. 
        // Let's UPDATE the query to join Users.

        const string sql = @"
            SELECT 
                ut.Id,
                ut.UserId,
                ut.TenantId,
                ut.Role,
                ut.IsActive
            FROM UserTenants ut
            INNER JOIN Users u ON ut.UserId = u.Id
            WHERE u.Email = @Email";

        using var db = connectionFactory.CreateMasterConnection();

        return await db.QueryFirstOrDefaultAsync<UserTenant>(
            sql,
            new { Email = email }
        );
    }

    public async Task<IEnumerable<UserTenant>> GetByUserIdAsync(Guid userId)
    {
        const string sql = @"
            SELECT 
                Id,
                UserId,
                TenantId,
                Role,
                IsActive
            FROM UserTenants
            WHERE UserId = @UserId";

        using var db = connectionFactory.CreateMasterConnection();

        return await db.QueryAsync<UserTenant>(
           sql,
           new { UserId = userId }
       );
    }

    public async Task<UserTenant> CadastrarAsync(UserTenant userTenant)
    {
        const string sql = @"
            INSERT INTO UserTenants (Id, UserId, TenantId, Role, IsActive)
            VALUES (@Id, @UserId, @TenantId, @Role, @IsActive);
            SELECT * FROM UserTenants WHERE Id = @Id;";

        using var db = connectionFactory.CreateMasterConnection();
        return await db.QuerySingleAsync<UserTenant>(sql, userTenant);
    }
}
