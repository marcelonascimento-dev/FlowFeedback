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
    public async Task<IActionResult> ObterAnaliseUnidade(Guid unidadeId, [FromQuery] DateTime? inicio, [FromQuery] DateTime? fim)
    {
        var query = _context.Votos.AsNoTracking().Where(v => v.UnidadeId == unidadeId);

        if (inicio.HasValue) query = query.Where(v => v.DataHoraVoto >= inicio.Value);
        if (fim.HasValue) query = query.Where(v => v.DataHoraVoto <= fim.Value);

        var votos = await query.ToListAsync();
        if (!votos.Any()) return NotFound("Sem dados para o período.");

        return Ok(ProcessarMetricas("Geral da Unidade", votos));
    }

    [HttpGet("nps/alvos/{unidadeId}")]
    public async Task<IActionResult> ObterNpsPorAlvos(Guid unidadeId)
    {
        var alvosInfo = await _context.AlvosAvaliacao
            .Where(a => a.UnidadeId == unidadeId)
            .ToDictionaryAsync(a => a.Id, a => a.Titulo);

        var votos = await _context.Votos.AsNoTracking().Where(v => v.UnidadeId == unidadeId).ToListAsync();

        var relatorio = votos
            .GroupBy(v => v.AlvoAvaliacaoId)
            .Select(g => ProcessarMetricas(alvosInfo.GetValueOrDefault(g.Key, "Desconhecido"), g.ToList()))
            .OrderByDescending(r => r.Nps);

        return Ok(relatorio);
    }

    private static ResumoNpsDto ProcessarMetricas(string nome, List<Domain.Entities.Voto> todosVotos)
    {
        // Filtro inteligente: desconsidera nota <= 2 sem comentário (ruído)
        var validos = todosVotos.Where(v => !(v.Nota <= 2 && v.Tags?.Any() == false)).ToList();

        int total = validos.Count;
        if (total == 0) return new ResumoNpsDto(nome, 0, 0, 0, 0, 0, 0, 0, 0);

        var notas = validos.Select(v => (double)v.Nota).OrderBy(n => n).ToList();
        double media = notas.Average();
        double mediana = notas.Count % 2 == 0
            ? (notas[notas.Count / 2 - 1] + notas[notas.Count / 2]) / 2
            : notas[notas.Count / 2];

        int promotores = validos.Count(v => v.Classificacao == ClassificacaoNps.Promotor);
        int detratores = validos.Count(v => v.Classificacao == ClassificacaoNps.Detrator);
        double nps = (double)(promotores - detratores) / total * 100;

        double confianca = (double)validos.Count / todosVotos.Count * 100;

        return new ResumoNpsDto(nome, total, Math.Round(media, 2), mediana, Math.Round(nps, 2),
                                promotores, total - (promotores + detratores), detratores, Math.Round(confianca, 2));
    }
}