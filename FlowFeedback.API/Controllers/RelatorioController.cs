using FlowFeedback.Application.DTOs;
using FlowFeedback.Domain.Enums;
using FlowFeedback.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlowFeedback.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RelatorioController : ControllerBase
{
    private readonly AppDbContext _context;

    public RelatorioController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("nps/unidade/{unidadeId}")]
    public async Task<IActionResult> ObterNpsPorUnidade(Guid unidadeId, [FromQuery] DateTime? inicio, [FromQuery] DateTime? fim)
    {
        var query = _context.Votos.AsNoTracking().Where(v => v.UnidadeId == unidadeId);

        if (inicio.HasValue) query = query.Where(v => v.DataHoraVoto >= inicio.Value);
        if (fim.HasValue) query = query.Where(v => v.DataHoraVoto <= fim.Value);

        var votos = await query.ToListAsync();

        if (!votos.Any()) return NotFound("Nenhum voto encontrado no período.");

        var resumo = CalcularMetricasNps("Relatório da Unidade", votos);
        return Ok(resumo);
    }

    [HttpGet("nps/alvos/{unidadeId}")]
    public async Task<IActionResult> ObterNpsPorAlvos(Guid unidadeId)
    {
        var alvosInfo = await _context.AlvosAvaliacao
            .Where(a => a.UnidadeId == unidadeId)
            .ToDictionaryAsync(a => a.Id, a => a.Titulo);

        var votos = await _context.Votos
            .AsNoTracking()
            .Where(v => v.UnidadeId == unidadeId)
            .ToListAsync();

        var relatorioPorAlvo = votos
            .GroupBy(v => v.AlvoAvaliacaoId)
            .Select(g => CalcularMetricasNps(
                alvosInfo.GetValueOrDefault(g.Key, "Alvo Excluído"),
                g.ToList()
            ))
            .OrderByDescending(r => r.Nps);

        return Ok(relatorioPorAlvo);
    }

    [HttpGet("analitico/unidade/{unidadeId}")]
    public async Task<IActionResult> ObterAnaliseInteligente(Guid unidadeId)
    {
        var votos = await _context.Votos
            .AsNoTracking()
            .Where(v => v.UnidadeId == unidadeId)
            .ToListAsync();

        if (!votos.Any()) return NotFound();

        // 1. Filtragem de "Ruído" (Votos nota 1 ou 2 sem motivo)
        var votosValidos = votos.Where(v => !(v.Nota <= 2 && string.IsNullOrEmpty(v.TagMotivo))).ToList();
        var votosInvalidados = votos.Count - votosValidos.Count;

        // 2. Cálculo de Média e Mediana
        var notas = votosValidos.Select(v => (double)v.Nota).OrderBy(n => n).ToList();
        double mediana = CalcularMediana(notas);
        double media = notas.Average();

        // 3. Cálculo de NPS (sobre os votos válidos)
        var total = votosValidos.Count;
        var nps = total == 0 ? 0 :
            ((double)(votosValidos.Count(v => v.Classificacao == ClassificacaoNps.Promotor) -
                      votosValidos.Count(v => v.Classificacao == ClassificacaoNps.Detrator)) / total) * 100;

        return Ok(new
        {
            Metricas = new
            {
                Media = Math.Round(media, 2),
                Mediana = mediana,
                NpsSugerido = Math.Round(nps, 2)
            },
            SaudeDosDados = new
            {
                TotalRecebido = votos.Count,
                VotosDesconsideradosPorFaltaDeMotivo = votosInvalidados,
                PercentualConfianca = Math.Round(((double)votosValidos.Count / votos.Count) * 100, 2)
            }
        });
    }

    private double CalcularMediana(List<double> notas)
    {
        int count = notas.Count;
        if (count == 0) return 0;

        if (count % 2 == 0)
            return (notas[(count / 2) - 1] + notas[count / 2]) / 2;

        return notas[count / 2];
    }

    private static ResumoNpsDto CalcularMetricasNps(string nome, List<Domain.Entities.Voto> votos)
    {
        int total = votos.Count;
        int promotores = votos.Count(v => v.Classificacao == ClassificacaoNps.Promotor);
        int neutros = votos.Count(v => v.Classificacao == ClassificacaoNps.Neutro);
        int detratores = votos.Count(v => v.Classificacao == ClassificacaoNps.Detrator);

        double nps = (double)(promotores - detratores) / total * 100;
        double media = votos.Average(v => v.Nota);

        return new ResumoNpsDto(nome, total, Math.Round(media, 2), Math.Round(nps, 2), promotores, neutros, detratores);
    }
}