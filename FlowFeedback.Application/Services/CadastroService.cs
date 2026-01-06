using FlowFeedback.Application.DTOs;
using FlowFeedback.Application.Interfaces;
using FlowFeedback.Domain.Entities;
using FlowFeedback.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FlowFeedback.Application.Services;

public class CadastroService(AppDbContext context) : ICadastroService
{
    private readonly AppDbContext _context = context;

    public async Task<Tenant> CadastrarTenantAsync(CreateTenantDto dto)
    {
        var tenant = new Tenant(dto.NomeCorporativo, dto.Cnpj);

        _context.Tenants.Add(tenant);
        await _context.SaveChangesAsync();
        return tenant;
    }

    public async Task<Unidade> CadastrarUnidadeAsync(CreateUnidadeDto dto)
    {
        var tenantExists = await _context.Tenants.AnyAsync(t => t.Id == dto.TenantId);
        if (!tenantExists) throw new ArgumentException("Tenant informado não existe.");

        var unidade = new Unidade(dto.TenantId, dto.Nome, dto.Endereco, dto.Cidade)
        {
            LogoUrlOverride = dto.LogoUrlOverride,
            CorPrimariaOverride = dto.CorPrimariaOverride,
            CorSecundariaOverride = dto.CorSecundariaOverride
        };

        _context.Unidades.Add(unidade);
        await _context.SaveChangesAsync();
        return unidade;
    }

    public async Task<Dispositivo> CadastrarDispositivoAsync(CreateDispositivoDto dto)
    {
        var unidadeExists = await _context.Unidades.AnyAsync(u => u.Id == dto.UnidadeId);
        if (!unidadeExists) throw new ArgumentException("Unidade informada não existe.");

        var dispositivo = new Dispositivo(dto.Identificador, dto.TenantId, dto.UnidadeId, dto.Nome);

        _context.Dispositivos.Add(dispositivo);
        await _context.SaveChangesAsync();
        return dispositivo;
    }

    public async Task<AlvoAvaliacao> CadastrarAlvoAvaliacaoAsync(CreateAlvoAvaliacaoDto dto)
    {
        var unidadeExists = await _context.Unidades.AnyAsync(u => u.Id == dto.UnidadeId);
        if (!unidadeExists) throw new ArgumentException("Unidade informada não existe.");

        var alvo = new AlvoAvaliacao(dto.UnidadeId, dto.Titulo, dto.Subtitulo, dto.ImagemUrl, dto.Tipo, dto.Ordem ?? 0);

        _context.AlvosAvaliacao.Add(alvo);
        await _context.SaveChangesAsync();
        return alvo;
    }

    public async Task VincularAlvoAvaliacaoADispositivoAsync(string dispositivoId, IEnumerable<string> alvosId)
    {
        var dispositivo = await _context.Dispositivos
            .Include(d => d.Alvos)
            .FirstOrDefaultAsync(d => d.Id == dispositivoId);

        if (dispositivo == null)
        {
            throw new Exception("Dispositivo não encontrado.");
        }

        dispositivo.Alvos.Clear();

        var novosAlvos = await _context.AlvosAvaliacao
            .Where(a => alvosId.Contains(a.Id.ToString()))
            .ToListAsync();

        foreach (var alvo in novosAlvos)
        {
            dispositivo.Alvos.Add(alvo);
        }

        await _context.SaveChangesAsync();
    }
}