using FlowFeedback.Domain.Entities;
using FlowFeedback.Domain.Enums;
using FlowFeedback.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlowFeedback.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SeedController : ControllerBase
{
    private readonly AppDbContext _context;

    public SeedController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("gerar-dados-teste")]
    public async Task<IActionResult> GerarDados()
    {
        _context.Votos.RemoveRange(await _context.Votos.ToListAsync());
        _context.AlvosAvaliacao.RemoveRange(await _context.AlvosAvaliacao.ToListAsync());
        _context.Dispositivos.RemoveRange(await _context.Dispositivos.ToListAsync());
        _context.Unidades.RemoveRange(await _context.Unidades.ToListAsync());
        _context.Tenants.RemoveRange(await _context.Tenants.ToListAsync());

        await _context.SaveChangesAsync();

        var tenant = new Tenant("Grupo Max Supermercados", "12.345.678/0001-99");
        await _context.Tenants.AddAsync(tenant);
        await _context.SaveChangesAsync();

        var unidadeCentro = new Unidade(tenant.Id, "Max - Loja Centro", "Goiânia", "Rua 10, 123");
        var unidadeShopping = new Unidade(tenant.Id, "Max - Loja Shopping", "Goiânia", "Av. T-63, S/N");

        await _context.Unidades.AddRangeAsync(unidadeCentro, unidadeShopping);
        await _context.SaveChangesAsync();

        var deviceAcougue = new Dispositivo(Guid.NewGuid(), tenant.Id, unidadeCentro.Id, "Totem Açougue");
        var deviceCheckin = new Dispositivo(Guid.NewGuid(), tenant.Id, unidadeShopping.Id, "Totem Entrada");

        await _context.Dispositivos.AddRangeAsync(deviceAcougue, deviceCheckin);

        var alvos = new List<AlvoAvaliacao>
        {
            new AlvoAvaliacao(unidadeCentro.Id, "João Silva", "Açougueiro", "https://i.pravatar.cc/300?img=11", TipoAlvo.Pessoa, 1),
            new AlvoAvaliacao(unidadeCentro.Id, "Limpeza Açougue", "Organização", null, TipoAlvo.Ambiente, 2),

            new AlvoAvaliacao(unidadeShopping.Id, "Atendimento Geral", "Cordialidade", null, TipoAlvo.Servico, 1),
            new AlvoAvaliacao(unidadeShopping.Id, "Maria Oliveira", "Recepcionista", "https://i.pravatar.cc/300?img=5", TipoAlvo.Pessoa, 2)
        };

        await _context.AlvosAvaliacao.AddRangeAsync(alvos);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            mensagem = "Estrutura complexa gerada com sucesso!",
            tenantId = tenant.Id,
            dispositivos = new[] {
                new { nome = deviceAcougue.NomeLocal, id = deviceAcougue.Id },
                new { nome = deviceCheckin.NomeLocal, id = deviceCheckin.Id }
            }
        });
    }
}