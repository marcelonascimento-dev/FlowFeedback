using System.Data;
using Dapper;
using FlowFeedback.Domain.Entities;
using FlowFeedback.Domain.Interfaces;
using FlowFeedback.Infrastructure.Data;

namespace FlowFeedback.Infrastructure.Repositories;

public class CadastroRepository(IDbConnectionFactory dbFactory) : ICadastroRepository
{
    public async Task<Tenant> AddTenantAsync(Tenant tenant)
    {
        using var db = dbFactory.CreateTenantConnection();
        var sql = @"INSERT INTO Tenants (Id, Nome, Documento, LogoUrl, CorPrimaria, CorSecundaria, Ativo, DataCriacao) 
                    VALUES (@Id, @Nome, @Documento, @LogoUrl, @CorPrimaria, @CorSecundaria, @Ativo, @DataCriacao)";

        await db.ExecuteAsync(sql, tenant);
        return tenant;
    }

    public async Task<Tenant?> GetTenantByIdAsync(Guid id)
    {
        using var db = dbFactory.CreateTenantConnection();
        return await db.QueryFirstOrDefaultAsync<Tenant>("SELECT * FROM Tenants WHERE Id = @Id", new { Id = id });
    }

    public async Task<Unidade> AddUnidadeAsync(Unidade unidade)
    {
        using var db = dbFactory.CreateTenantConnection();
        var sql = @"INSERT INTO Unidades (Id, TenantId, Nome, Cidade, Endereco, LogoUrlOverride, CorPrimariaOverride, CorSecundariaOverride, Ativo) 
                    VALUES (@Id, @TenantId, @Nome, @Cidade, @Endereco, @LogoUrlOverride, @CorPrimariaOverride, @CorSecundariaOverride, @Ativo)";

        await db.ExecuteAsync(sql, unidade);
        return unidade;
    }

    public async Task<Unidade?> GetUnidadeByIdAsync(Guid id)
    {
        using var db = dbFactory.CreateTenantConnection();
        var sql = @"SELECT u.*, t.* FROM Unidades u 
                    INNER JOIN Tenants t ON u.TenantId = t.Id 
                    WHERE u.Id = @Id";

        return (await db.QueryAsync<Unidade, Tenant, Unidade>(
            sql,
            (unidade, tenant) => {
                unidade.Tenant = tenant;
                return unidade;
            },
            new { Id = id },
            splitOn: "Id")).FirstOrDefault();
    }

    public async Task<AlvoAvaliacao> AddAlvoAsync(AlvoAvaliacao alvo)
    {
        using var db = dbFactory.CreateTenantConnection();
        var sql = @"INSERT INTO AlvosAvaliacao (Id, UnidadeId, TenantId, Nome, Subtitulo, ImagemUrl, Ordem, Tipo, Ativo) 
                    VALUES (@Id, @UnidadeId, @TenantId, @Nome, @Subtitulo, @ImagemUrl, @Ordem, @Tipo, @Ativo)";

        await db.ExecuteAsync(sql, alvo);
        return alvo;
    }

    public async Task<AlvoAvaliacao?> GetAlvoByIdAsync(Guid id)
    {
        using var db = dbFactory.CreateTenantConnection();
        return await db.QueryFirstOrDefaultAsync<AlvoAvaliacao>("SELECT * FROM AlvosAvaliacao WHERE Id = @Id", new { Id = id });
    }


}