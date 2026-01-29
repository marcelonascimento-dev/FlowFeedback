using Dapper;
using FlowFeedback.Domain.Entities;
using FlowFeedback.Domain.Interfaces;
using FlowFeedback.Infrastructure.Data;

namespace FlowFeedback.Infrastructure.Repositories;

public class UsuarioRepository(IDbConnectionFactory dbFactory) : IUsuarioRepository
{
    public async Task<Usuario?> ObterPorEmailAsync(string email)
    {
        using var conn = dbFactory.CreateMasterConnection();

        return await conn.QueryFirstOrDefaultAsync<Usuario>(
            @"SELECT Id, TenantCode, Nome, Email, SenhaHash, Role, Ativo 
              FROM Usuarios 
              WHERE Email = @Email AND Ativo = 1",
            new { Email = email });
    }

    public async Task<Usuario?> ObterPorIdAsync(Guid id)
    {
        const string sql = @"
            SELECT 
                Id,
                Email,
                SenhaHash,
                Role,
                Ativo
            FROM Usuarios
            WHERE Id = @Id";

        using var db = dbFactory.CreateTenantConnection();

        return await db.QueryFirstOrDefaultAsync<Usuario>(
            sql,
            new { Id = id }
        );
    }
}