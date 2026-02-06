using Dapper;
using FlowFeedback.Domain.Entities;
using FlowFeedback.Domain.Interfaces;
using FlowFeedback.Infrastructure.Data;

namespace FlowFeedback.Infrastructure.Repositories;

public class AlvoAvaliacaoRepository(IDbConnectionFactory dbFactory) : IAlvoAvaliacaoRepository
{
    public async Task<IEnumerable<AlvoAvaliacao>> GetAllAsync()
    {
        using var db = dbFactory.CreateTenantConnection();
        return await db.QueryAsync<AlvoAvaliacao>("SELECT * FROM AlvosAvaliacao");
    }

    public async Task<AlvoAvaliacao?> GetByIdAsync(Guid id)
    {
        using var db = dbFactory.CreateTenantConnection();
        return await db.QueryFirstOrDefaultAsync<AlvoAvaliacao>("SELECT * FROM AlvosAvaliacao WHERE Id = @Id", new { Id = id });
    }

    public async Task AddAsync(AlvoAvaliacao alvo)
    {
        using var db = dbFactory.CreateTenantConnection();
        var sql = @"INSERT INTO AlvosAvaliacao (Id, EmpresaId, Nome, Subtitulo, ImagemUrl, Ordem, Tipo, Ativo) 
                    VALUES (@Id, @EmpresaId, @Nome, @Subtitulo, @ImagemUrl, @Ordem, @Tipo, @Ativo)";

        await db.ExecuteAsync(sql, alvo);
    }

    public async Task UpdateAsync(AlvoAvaliacao alvo)
    {
        using var db = dbFactory.CreateTenantConnection();
        var sql = @"UPDATE AlvosAvaliacao SET 
                        EmpresaId = @EmpresaId, 
                        Nome = @Nome, 
                        Subtitulo = @Subtitulo, 
                        ImagemUrl = @ImagemUrl, 
                        Ordem = @Ordem, 
                        Tipo = @Tipo, 
                        Ativo = @Ativo 
                    WHERE Id = @Id";

        await db.ExecuteAsync(sql, alvo);
    }

    public async Task DeleteAsync(Guid id)
    {
        using var db = dbFactory.CreateTenantConnection();
        await db.ExecuteAsync("DELETE FROM AlvosAvaliacao WHERE Id = @Id", new { Id = id });
    }
}
