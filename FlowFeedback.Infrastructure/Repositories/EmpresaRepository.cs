using Dapper;
using FlowFeedback.Domain.Entities;
using FlowFeedback.Domain.Interfaces;
using FlowFeedback.Infrastructure.Data;

namespace FlowFeedback.Infrastructure.Repositories;

public class EmpresaRepository(IDbConnectionFactory dbFactory) : IEmpresaRepository
{
    public async Task<IEnumerable<Empresa>> GetAllAsync()
    {
        using var db = dbFactory.CreateTenantConnection();
        return await db.QueryAsync<Empresa>("SELECT * FROM Empresas");
    }

    public async Task<Empresa?> GetByIdAsync(Guid id)
    {
        using var db = dbFactory.CreateTenantConnection();
        return await db.QueryFirstOrDefaultAsync<Empresa>("SELECT * FROM Empresas WHERE Id = @Id", new { Id = id });
    }

    public async Task AddAsync(Empresa empresa)
    {
        using var db = dbFactory.CreateTenantConnection();
        var sql = @"INSERT INTO Empresas (Id, Nome, CNPJ, Email, Telefone, CEP, Logradouro, Numero, Complemento, Bairro, Cidade, UF, LogoUrlOverride, CorPrimariaOverride, CorSecundariaOverride, Ativo) 
                    VALUES (@Id, @Nome, @CNPJ, @Email, @Telefone, @CEP, @Logradouro, @Numero, @Complemento, @Bairro, @Cidade, @UF, @LogoUrlOverride, @CorPrimariaOverride, @CorSecundariaOverride, @Ativo)";

        await db.ExecuteAsync(sql, empresa);
    }

    public async Task UpdateAsync(Empresa empresa)
    {
        using var db = dbFactory.CreateTenantConnection();
        var sql = @"UPDATE Empresas SET 
                        Nome = @Nome, 
                        CNPJ = @CNPJ, 
                        Email = @Email, 
                        Telefone = @Telefone, 
                        CEP = @CEP, 
                        Logradouro = @Logradouro, 
                        Numero = @Numero, 
                        Complemento = @Complemento, 
                        Bairro = @Bairro, 
                        Cidade = @Cidade, 
                        UF = @UF, 
                        LogoUrlOverride = @LogoUrlOverride, 
                        CorPrimariaOverride = @CorPrimariaOverride, 
                        CorSecundariaOverride = @CorSecundariaOverride, 
                        Ativo = @Ativo 
                    WHERE Id = @Id";

        await db.ExecuteAsync(sql, empresa);
    }

    public async Task DeleteAsync(Guid id)
    {
        using var db = dbFactory.CreateTenantConnection();
        await db.ExecuteAsync("DELETE FROM Empresas WHERE Id = @Id", new { Id = id });
    }
}
