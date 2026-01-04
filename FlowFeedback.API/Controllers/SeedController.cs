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
        _context.Dispositivos.RemoveRange(await _context.Dispositivos.ToListAsync());
        _context.Unidades.RemoveRange(await _context.Unidades.ToListAsync());
        _context.AlvosAvaliacao.RemoveRange(await _context.AlvosAvaliacao.ToListAsync());

        await _context.SaveChangesAsync();

        var tenantId = Guid.NewGuid();

        var unidadeCentro = new Unidade(tenantId, "Max Supermercado - Centro", "LJ-001");
        var unidadeShopping = new Unidade(tenantId, "Max Supermercado - Shopping", "LJ-002");

        await _context.Unidades.AddRangeAsync(unidadeCentro, unidadeShopping);
        await _context.SaveChangesAsync();

        var deviceAcougueCentro = new Dispositivo(Guid.NewGuid(), tenantId, unidadeCentro.Id, "Totem Açougue");
        var deviceCaixaShopping = new Dispositivo(Guid.NewGuid(), tenantId, unidadeShopping.Id, "Totem Saída");

        await _context.Dispositivos.AddRangeAsync(deviceAcougueCentro, deviceCaixaShopping);

        var alvos = new List<AlvoAvaliacao>
        {
            new AlvoAvaliacao(tenantId, "João Silva", "Açougueiro", "https://i.pravatar.cc/300?img=11", TipoAlvo.Pessoa, 1),
            new AlvoAvaliacao(tenantId, "Maria Oliveira", "Atendente", "https://i.pravatar.cc/300?img=5", TipoAlvo.Pessoa, 2),
            new AlvoAvaliacao(tenantId, "Limpeza Geral", "Como está o ambiente?", "https://cdn-icons-png.flaticon.com/512/2059/2059806.png", TipoAlvo.Ambiente, 3),
            new AlvoAvaliacao(tenantId, "Tempo de Fila", "Foi rápido?", "https://cdn-icons-png.flaticon.com/512/3063/3063069.png", TipoAlvo.Servico, 4)
        };

        await _context.AlvosAvaliacao.AddRangeAsync(alvos);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            mensagem = "Dados de hierarquia gerados",
            tenantId,
            unidades = new
            {
                centro = new { id = unidadeCentro.Id, nome = unidadeCentro.Nome },
                shopping = new { id = unidadeShopping.Id, nome = unidadeShopping.Nome }
            },
            dispositivos = new
            {
                acougueCentro = deviceAcougueCentro.Id,
                saidaShopping = deviceCaixaShopping.Id
            }
        });
    }
}