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

    public async Task<bool> CriarSchemaTenantAsync(Guid tenantId)
    {
        var schemaName = $"tenant_{tenantId:N}";
        using var masterConn = dbFactory.CreateMasterConnection();
        await masterConn.ExecuteAsync($"CREATE DATABASE [{schemaName}]");

        var scriptTabelas = ScriptProvider.GetTenantSchemaScript();
        
        return await masterConn.ExecuteAsync(scriptTabelas) == 1;
    }

    public async Task<Tenant?> GetTenantByIdAsync(Guid id)
    {
        using var db = dbFactory.CreateTenantConnection();
        return await db.QueryFirstOrDefaultAsync<Tenant>("SELECT * FROM Tenants WHERE Id = @Id", new { Id = id });
    }

    public async Task<Empresa> AddEmpresaAsync(Empresa Empresa)
    {
        using var db = dbFactory.CreateTenantConnection();
        var sql = @"INSERT INTO Empresas (Id, TenantId, Nome, Cidade, Endereco, LogoUrlOverride, CorPrimariaOverride, CorSecundariaOverride, Ativo) 
                    VALUES (@Id, @TenantId, @Nome, @Cidade, @Endereco, @LogoUrlOverride, @CorPrimariaOverride, @CorSecundariaOverride, @Ativo)";

        await db.ExecuteAsync(sql, Empresa);
        return Empresa;
    }

    public async Task<Empresa?> GetEmpresaByIdAsync(Guid id)
    {
        using var db = dbFactory.CreateTenantConnection();
        var sql = @"SELECT u.*, t.* FROM Empresas u 
                    INNER JOIN Tenants t ON u.TenantId = t.Id 
                    WHERE u.Id = @Id";

        return (await db.QueryAsync<Empresa, Tenant, Empresa>(
            sql,
            (Empresa, tenant) => {
                Empresa.Tenant = tenant;
                return Empresa;
            },
            new { Id = id },
            splitOn: "Id")).FirstOrDefault();
    }

    public async Task<AlvoAvaliacao> AddAlvoAsync(AlvoAvaliacao alvo)
    {
        using var db = dbFactory.CreateTenantConnection();
        var sql = @"INSERT INTO AlvosAvaliacao (Id, EmpresaId, TenantId, Nome, Subtitulo, ImagemUrl, Ordem, Tipo, Ativo) 
                    VALUES (@Id, @EmpresaId, @TenantId, @Nome, @Subtitulo, @ImagemUrl, @Ordem, @Tipo, @Ativo)";

        await db.ExecuteAsync(sql, alvo);
        return alvo;
    }

    public async Task<AlvoAvaliacao?> GetAlvoByIdAsync(Guid id)
    {
        using var db = dbFactory.CreateTenantConnection();
        return await db.QueryFirstOrDefaultAsync<AlvoAvaliacao>("SELECT * FROM AlvosAvaliacao WHERE Id = @Id", new { Id = id });
    }


}