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
            @"SELECT Id, Name, Email, PasswordHash, IsActive, EmailConfirmed, CreatedAt
              FROM Users
              WHERE Email = @Email AND IsActive = 1",
            new { Email = email });
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        const string sql = @"
            SELECT 
                Id,
                Name,
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
            INSERT INTO Users (Id, Name, Email, PasswordHash, IsActive, EmailConfirmed, CreatedAt)
            VALUES (@Id, @Name, @Email, @PasswordHash, @IsActive, @EmailConfirmed, @CreatedAt);
            SELECT * FROM Users WHERE Id = @Id;";

        using var db = dbFactory.CreateMasterConnection();
        return await db.QuerySingleAsync<User>(sql, user);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        const string sql = "SELECT Id, Name, Email, PasswordHash, IsActive, EmailConfirmed, CreatedAt FROM Users";
        using var db = dbFactory.CreateMasterConnection();
        return await db.QueryAsync<User>(sql);
    }

    public async Task UpdateAsync(User user)
    {
        const string sql = @"
            UPDATE Users 
            SET Name = @Name, 
                Email = @Email, 
                PasswordHash = @PasswordHash, 
                IsActive = @IsActive, 
                EmailConfirmed = @EmailConfirmed
            WHERE Id = @Id";

        using var db = dbFactory.CreateMasterConnection();
        await db.ExecuteAsync(sql, user);
    }

    public async Task DeleteAsync(Guid id)
    {
        const string sql = "DELETE FROM Users WHERE Id = @Id";
        using var db = dbFactory.CreateMasterConnection();
        await db.ExecuteAsync(sql, new { Id = id });
    }
}