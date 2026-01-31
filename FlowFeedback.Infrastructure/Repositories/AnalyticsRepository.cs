using Dapper;
using FlowFeedback.Domain.Interfaces;
using FlowFeedback.Domain.Models;
using FlowFeedback.Infrastructure.Data;

namespace FlowFeedback.Infrastructure.Repositories;

public class AnalyticsRepository(IDbConnectionFactory dbFactory) : IAnalyticsRepository
{
    public async Task<NpsScoreDto> GetNpsPeriodoAsync(Guid tenantId, DateTime inicio, DateTime fim)
    {
        using var db = dbFactory.CreateTenantConnection();

        var sql = @"
            SELECT 
                COUNT(1) as TotalVotos,
                SUM(CASE WHEN Valor >= 9 THEN 1 ELSE 0 END) as QtdPromotores,
                SUM(CASE WHEN Valor >= 7 AND Valor <= 8 THEN 1 ELSE 0 END) as QtdNeutros,
                SUM(CASE WHEN Valor <= 6 THEN 1 ELSE 0 END) as QtdDetratores
            FROM Votos
            WHERE TenantId = @TenantId 
              AND DataHora >= @Inicio 
              AND DataHora <= @Fim";

        var stats = await db.QueryFirstOrDefaultAsync<dynamic>(sql, new { TenantId = tenantId, Inicio = inicio, Fim = fim });

        if (stats == null || stats?.TotalVotos == 0)
        {
            return new NpsScoreDto { Score = 0, TotalVotos = 0 };
        }

        long total = stats?.TotalVotos;
        long promotores = stats?.QtdPromotores;
        long detratores = stats?.QtdDetratores;
        long neutros = stats?.QtdNeutros;

        double score = ((double)(promotores - detratores) / total) * 100;

        return new NpsScoreDto
        {
            Score = Math.Round(score, 1),
            TotalVotos = (int)total,
            QtdPromotores = (int)promotores,
            QtdDetratores = (int)detratores,
            QtdNeutros = (int)neutros
        };
    }

    public async Task<IEnumerable<NpsEvolucaoDiariaDto>> GetEvolucaoNpsAsync(Guid tenantId, DateTime inicio, DateTime fim)
    {
        using var db = dbFactory.CreateTenantConnection();

        var sql = @"
            SELECT 
                CAST(DataHora as DATE) as Data,
                COUNT(1) as TotalVotos,
                SUM(CASE WHEN Valor >= 9 THEN 1 ELSE 0 END) as Promotores,
                SUM(CASE WHEN Valor <= 6 THEN 1 ELSE 0 END) as Detratores
            FROM Votos
            WHERE TenantId = @TenantId 
              AND DataHora >= @Inicio 
              AND DataHora <= @Fim
            GROUP BY CAST(DataHora as DATE)
            ORDER BY Data";

        var dados = await db.QueryAsync<dynamic>(sql, new { TenantId = tenantId, Inicio = inicio, Fim = fim });

        return dados.Select(d =>
        {
            long prom = d.Promotores;
            long det = d.Detratores;
            long total = d.TotalVotos;
            double score = total == 0 ? 0 : ((double)(prom - det) / total) * 100;

            return new NpsEvolucaoDiariaDto((DateTime)d.Data, Math.Round(score, 1), (int)total);
        });
    }
}