using Dapper;
using FlowFeedback.Domain.Entities;
using FlowFeedback.Domain.Repositories;
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
}