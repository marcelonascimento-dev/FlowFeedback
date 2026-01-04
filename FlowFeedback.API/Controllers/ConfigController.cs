using FlowFeedback.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlowFeedback.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConfigController : ControllerBase
{
    private readonly AppDbContext _context;

    public ConfigController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("dispositivo/{deviceId}")]
    public async Task<IActionResult> ObterConfiguracaoPorDispositivo(Guid deviceId)
    {
        var dispositivo = await _context.Dispositivos
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == deviceId && d.Ativo);

        if (dispositivo == null)
            return NotFound("Dispositivo não encontrado ou inativo.");

        var unidade = await _context.Unidades
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == dispositivo.UnidadeId && u.Ativa);

        if (unidade == null)
            return NotFound("Unidade vinculada ao dispositivo não encontrada ou inativa.");

        var tenant = await _context.Tenants
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == unidade.TenantId);

        var alvos = await _context.AlvosAvaliacao
            .AsNoTracking()
            .Where(a => a.UnidadeId == unidade.Id && a.Ativo)
            .OrderBy(a => a.OrdemExibicao)
            .ToListAsync();

        return Ok(new
        {
            GrupoId = tenant?.Id,
            GrupoNome = tenant?.NomeCorporativo,
            UnidadeId = unidade.Id,
            UnidadeNome = unidade.NomeLoja,
            Cidade = unidade.Cidade,
            Cards = alvos.Select(a => new
            {
                a.Id,
                a.Titulo,
                a.Subtitulo,
                a.ImagemUrl,
                Tipo = (int)a.Tipo
            })
        });
    }

    [HttpGet("unidade/{unidadeId}")]
    public async Task<IActionResult> ObterConfiguracaoGeralUnidade(Guid unidadeId)
    {
        var unidade = await _context.Unidades
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == unidadeId);

        if (unidade == null)
            return NotFound("Unidade não encontrada.");

        var tenant = await _context.Tenants
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == unidade.TenantId);

        if (tenant == null)
            return NotFound("Grupo (Tenant) não encontrado para esta unidade.");

        var dispositivos = await _context.Dispositivos
            .AsNoTracking()
            .Where(d => d.UnidadeId == unidadeId)
            .ToListAsync();

        var response = new
        {
            Tenant = new
            {
                tenant.Id,
                tenant.NomeCorporativo,
                tenant.Cnpj,
                Status = tenant.Ativo ? "Ativo" : "Inativo"
            },
            Unidade = new
            {
                unidade.Id,
                unidade.NomeLoja,
                unidade.Cidade,
                unidade.Endereco,
                Status = unidade.Ativa ? "Ativa" : "Inativa",
                Dispositivos = dispositivos
            }
        };

        return Ok(response);
    }

    [HttpGet("tenant/{tenantId}")]
    public async Task<IActionResult> ObterConfiguracaoGrupo(Guid tenantId)
    {
        var tenant = await _context.Tenants
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == tenantId);

        if (tenant == null)
            return NotFound("Grupo (Tenant) não encontrado.");

        var unidades = await _context.Unidades
            .AsNoTracking()
            .Where(u => u.TenantId == tenantId)
            .Select(u => new
            {
                u.Id,
                u.NomeLoja,
                u.Cidade,
                u.Ativa,
                TotalDispositivos = _context.Dispositivos.Count(d => d.UnidadeId == u.Id)
            })
            .ToListAsync();

        var response = new
        {
            tenant.Id,
            tenant.NomeCorporativo,
            tenant.Cnpj,
            Status = tenant.Ativo ? "Ativo" : "Inativo",
            TotalUnidades = unidades.Count,
            Unidades = unidades
        };

        return Ok(response);
    }
}