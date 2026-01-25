using Dapper;
using FlowFeedback.Domain.Entities;
using FlowFeedback.Domain.Interfaces;
using FlowFeedback.Infrastructure.Data;

namespace FlowFeedback.Infrastructure.Repositories;

public class DispositivoRepository(IDbConnectionFactory dbFactory) : IDispositivoRepository
{
    public async Task<Dispositivo?> GetByIdentifierAsync(string deviceIdentifier)
    {
        string SqlSelectByIdentifier = @"
        SELECT Id, UnidadeId, TenantId, Nome, Identificador, Ativo, DataCriacao 
        FROM Dispositivos 
        WHERE Identificador = @Identificador";

        using var db = dbFactory.CreateTenantConnection();

        return await db.QueryFirstOrDefaultAsync<Dispositivo>(SqlSelectByIdentifier, new { Identificador = deviceIdentifier });
    }

    public async Task<Dispositivo> AddDispositivoAsync(Dispositivo dispositivo)
    {
        using var db = dbFactory.CreateTenantConnection();
        var sql = @"INSERT INTO Dispositivos (Id, UnidadeId, TenantId, Nome, Identificador, Ativo, DataCriacao) 
                    VALUES (@Id, @UnidadeId, @TenantId, @Nome, @Identificador, @Ativo, @DataCriacao)";

        await db.ExecuteAsync(sql, dispositivo);
        return dispositivo;
    }

    public async Task<bool> DispositivoExisteAsync(string identificador)
    {
        string SqlCheckExists = "SELECT CAST(COUNT(1) AS BIT) FROM Dispositivos WHERE Identificador = @Identificador";
        using var db = dbFactory.CreateTenantConnection();
        return await db.ExecuteScalarAsync<bool>(SqlCheckExists, new { Identificador = identificador });
    }

    public async Task<Dispositivo?> GetDispositivoWithAlvosAsync(Guid id)
    {
        using var db = dbFactory.CreateTenantConnection();
        var sql = @"
            SELECT d.*, a.* FROM Dispositivos d
            LEFT JOIN DispositivoAlvos da ON d.Id = da.DispositivoId
            LEFT JOIN AlvosAvaliacao a ON da.AlvoAvaliacaoId = a.Id
            WHERE d.Id = @Id";

        var dispositivoDictionary = new Dictionary<Guid, Dispositivo>();

        var result = await db.QueryAsync<Dispositivo, AlvoAvaliacao, Dispositivo>(
            sql,
            (dispositivo, alvo) =>
            {
                if (!dispositivoDictionary.TryGetValue(dispositivo.Id, out var currentDispositivo))
                {
                    currentDispositivo = dispositivo;
                    currentDispositivo.AlvosAvaliacao = [];
                    dispositivoDictionary.Add(currentDispositivo.Id, currentDispositivo);
                }

                if (alvo != null)
                {
                    currentDispositivo.AlvosAvaliacao.Add(alvo);
                }

                return currentDispositivo;
            },
            new { Id = id },
            splitOn: "Id"
        );

        return result.FirstOrDefault();
    }

    public async Task UpdateDispositivoAsync(Dispositivo dispositivo)
    {
        using var db = dbFactory.CreateTenantConnection();
        db.Open();
        using var trans = db.BeginTransaction();

        try
        {
            await db.ExecuteAsync(
                "UPDATE Dispositivos SET Nome = @Nome, Ativo = @Ativo WHERE Id = @Id",
                dispositivo,
                transaction: trans);

            await db.ExecuteAsync(
                "DELETE FROM DispositivoAlvos WHERE DispositivoId = @Id",
                new { Id = dispositivo.Id },
                transaction: trans);

            if (dispositivo.AlvosAvaliacao.Any())
            {
                var vinculos = dispositivo.AlvosAvaliacao.Select(a => new { DispositivoId = dispositivo.Id, AlvoAvaliacaoId = a.Id });
                await db.ExecuteAsync(
                    "INSERT INTO DispositivoAlvos (DispositivoId, AlvoAvaliacaoId) VALUES (@DispositivoId, @AlvoAvaliacaoId)",
                    vinculos,
                    transaction: trans);
            }

            trans.Commit();
        }
        catch
        {
            trans.Rollback();
            throw;
        }
    }
}