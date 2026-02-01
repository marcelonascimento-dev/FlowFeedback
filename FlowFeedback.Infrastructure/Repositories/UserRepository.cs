using Dapper;
using FlowFeedback.Domain.Entities;
using FlowFeedback.Domain.Interfaces;
using FlowFeedback.Infrastructure.Data;

namespace FlowFeedback.Infrastructure.Repositories;

public class UserRepository(IDbConnectionFactory dbFactory) : IUserRepository
{
    public async Task<User?> GetByEmailAsync(string email)
    {
        using var conn = dbFactory.CreateMasterConnection();

        return await conn.QueryFirstOrDefaultAsync<User>(
            @"SELECT Id, Email, PasswordHash, IsActive, EmailConfirmed, CreatedAt
              FROM Users
              WHERE Email = @Email AND IsActive = 1",
            new { Email = email });
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        const string sql = @"
            SELECT 
                Id,
                Email,
                PasswordHash,
                IsActive,
                EmailConfirmed,
                CreatedAt
            FROM Users
            WHERE Id = @Id";

        using var db = dbFactory.CreateMasterConnection(); // Users are in Master DB now

        return await db.QueryFirstOrDefaultAsync<User>(
            sql,
            new { Id = id }
        );
    }

    public async Task<User> CadastrarAsync(User user)
    {
        const string sql = @"
            INSERT INTO Users (Id, Email, PasswordHash, IsActive, EmailConfirmed, CreatedAt)
            VALUES (@Id, @Email, @PasswordHash, @IsActive, @EmailConfirmed, @CreatedAt);
            SELECT * FROM Users WHERE Id = @Id;";

        using var db = dbFactory.CreateMasterConnection();
        return await db.QuerySingleAsync<User>(sql, user);
    }
}