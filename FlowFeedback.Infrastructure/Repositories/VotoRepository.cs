using Dapper;
using FlowFeedback.Domain.Entities;
using FlowFeedback.Domain.Interfaces;
using FlowFeedback.Infrastructure.Data;

namespace FlowFeedback.Infrastructure.Repositories;

public class VotoRepository(IDbConnectionFactory dbFactory) : IVotoRepository
{
    public async Task<HashSet<Guid>> GetExistingIdsAsync(IEnumerable<Guid> idsToCheck)
    {
        using var db = dbFactory.CreateTenantConnection();

        var query = "SELECT Id FROM Votos WHERE Id IN @Ids";

        var result = await db.QueryAsync<Guid>(query, new { Ids = idsToCheck });
        return result.ToHashSet();
    }

    public async Task AdicionarLoteAsync(IEnumerable<Voto> votos)
    {
        using var db = dbFactory.CreateTenantConnection();

        var sql = @"
            INSERT INTO Votos (Id, EmpresaId, DispositivoId, Valor, Comentario, DataHora, DataProcessamento)
            VALUES (@Id, @EmpresaId, @DispositivoId, @Valor, @Comentario, @DataHora, @DataProcessamento)";

        await db.ExecuteAsync(sql, votos);
    }
}