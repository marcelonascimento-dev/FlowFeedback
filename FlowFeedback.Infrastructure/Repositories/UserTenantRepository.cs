using Dapper;
using FlowFeedback.Domain.Entities;
using FlowFeedback.Domain.Interfaces;
using FlowFeedback.Infrastructure.Data;

namespace FlowFeedback.Infrastructure.Repositories;

public sealed class UserTenantRepository(IDbConnectionFactory connectionFactory) : IUserTenantRepository
{
    public async Task<UserTenant?> GetByEmailAsync(string email)
    {
        const string sql = @"
            SELECT 
                ut.Id, ut.UserId, ut.TenantId, ut.Role, ut.IsActive,
                u.Id, u.Name, u.Email, u.PasswordHash, u.IsActive,
                t.Id, t.Name, t.Slug, t.DbServer, t.DbName, t.DbUser, t.DbPassword, t.Status, t.CreatedAt
            FROM UserTenants ut
            INNER JOIN Users u ON ut.UserId = u.Id
            INNER JOIN Tenants t ON ut.TenantId = t.Id
            WHERE u.Email = @Email";

        using var db = connectionFactory.CreateMasterConnection();

        var result = await db.QueryAsync<UserTenant, User, Tenant, UserTenant>(
            sql,
            (userTenant, user, tenant) =>
            {
                userTenant.User = user;
                userTenant.Tenant = tenant;
                return userTenant;
            },
            new { Email = email },
            splitOn: "Id,Id"
        );

        return result.FirstOrDefault();
    }

    public async Task<IEnumerable<UserTenant>> GetByUserIdAsync(Guid userId)
    {
        const string sql = @"
            SELECT 
                ut.Id, ut.UserId, ut.TenantId, ut.Role, ut.IsActive,
                u.Id, u.Name, u.Email, u.PasswordHash, u.IsActive,
                t.Id, t.Name, t.Slug, t.DbServer, t.DbName, t.DbUser, t.DbPassword, t.Status, t.CreatedAt
            FROM UserTenants ut
            INNER JOIN Users u ON ut.UserId = u.Id
            INNER JOIN Tenants t ON ut.TenantId = t.Id
            WHERE ut.UserId = @UserId";

        using var db = connectionFactory.CreateMasterConnection();

        return await db.QueryAsync<UserTenant, User, Tenant, UserTenant>(
            sql,
            (userTenant, user, tenant) =>
            {
                userTenant.User = user;
                userTenant.Tenant = tenant;
                return userTenant;
            },
            new { UserId = userId },
            splitOn: "Id,Id"
        );
    }

    public async Task<UserTenant?> GetByIdsAsync(Guid userId, Guid tenantId)
    {
        const string sql = @"
            SELECT 
                ut.Id, ut.UserId, ut.TenantId, ut.Role, ut.IsActive,
                u.Id, u.Name, u.Email, u.PasswordHash, u.IsActive,
                t.Id, t.Name, t.Slug, t.DbServer, t.DbName, t.DbUser, t.DbPassword, t.Status, t.CreatedAt
            FROM UserTenants ut
            INNER JOIN Users u ON ut.UserId = u.Id
            INNER JOIN Tenants t ON ut.TenantId = t.Id
            WHERE ut.UserId = @UserId AND ut.TenantId = @TenantId";

        using var db = connectionFactory.CreateMasterConnection();

        var result = await db.QueryAsync<UserTenant, User, Tenant, UserTenant>(
            sql,
            (userTenant, user, tenant) =>
            {
                userTenant.User = user;
                userTenant.Tenant = tenant;
                return userTenant;
            },
            new { UserId = userId, TenantId = tenantId },
            splitOn: "Id,Id"
        );

        return result.FirstOrDefault();
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
