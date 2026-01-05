using FlowFeedback.Domain.Entities;
using FlowFeedback.Domain.Enums;
using FlowFeedback.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace FlowFeedback.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SeedController(AppDbContext context) : ControllerBase
{
    [HttpPost("gerar-dados-teste")]
    public async Task<IActionResult> GerarDados()
    {
        // Limpeza (Mantendo a lógica anterior)
        context.Votos.RemoveRange(await context.Votos.ToListAsync());
        context.AlvosAvaliacao.RemoveRange(await context.AlvosAvaliacao.ToListAsync());
        context.Dispositivos.RemoveRange(await context.Dispositivos.ToListAsync());
        context.Unidades.RemoveRange(await context.Unidades.ToListAsync());
        context.Tenants.RemoveRange(await context.Tenants.ToListAsync());
        await context.SaveChangesAsync();

        // 1. Criar estrutura base
        var tenant = new Tenant("Grupo Max Supermercados", "12.345.678/0001-99");
        await context.Tenants.AddAsync(tenant);
        await context.SaveChangesAsync();

        var unidadeCentro = new Unidade(tenant.Id, "Max - Loja Centro", "Goiânia", "Rua 10, 123");
        await context.Unidades.AddAsync(unidadeCentro);
        await context.SaveChangesAsync();

        // 2. Dispositivos (Totens)
        var deviceAcougue = new Dispositivo("PC-1967_OPTIPLEX_7060", tenant.Id, unidadeCentro.Id, "Totem Açougue");
        var deviceHorti = new Dispositivo("HT_01_TESTE", tenant.Id, unidadeCentro.Id, "Totem Hortifrúti");
        await context.Dispositivos.AddRangeAsync(deviceAcougue, deviceHorti);

        // 3. Funcionários do Açougue
        var funcA1 = new AlvoAvaliacao(unidadeCentro.Id, "João Silva", "Mestre Açougueiro", "https://i.pravatar.cc/300?img=11", TipoAlvo.Pessoa, 1);
        var funcA2 = new AlvoAvaliacao(unidadeCentro.Id, "Ricardo Souza", "Atendente Açougue", "https://i.pravatar.cc/300?img=12", TipoAlvo.Pessoa, 2);
        var funcA3 = new AlvoAvaliacao(unidadeCentro.Id, "Marcos Vinícius", "Auxiliar", "https://i.pravatar.cc/300?img=13", TipoAlvo.Pessoa, 3);
        var limpezaAcougue = new AlvoAvaliacao(unidadeCentro.Id, "Limpeza do Setor", "Açougue", null, TipoAlvo.Ambiente, 4);

        // 4. Funcionários do Hortifrúti
        var funcH1 = new AlvoAvaliacao(unidadeCentro.Id, "Maria Oliveira", "Líder Horti", "https://i.pravatar.cc/300?img=5", TipoAlvo.Pessoa, 1);
        var funcH2 = new AlvoAvaliacao(unidadeCentro.Id, "Ana Costa", "Reposição", "https://i.pravatar.cc/300?img=48", TipoAlvo.Pessoa, 2);
        var funcH3 = new AlvoAvaliacao(unidadeCentro.Id, "Patrícia Lima", "Atendimento", "https://i.pravatar.cc/300?img=47", TipoAlvo.Pessoa, 3);
        var qualidadeFrutas = new AlvoAvaliacao(unidadeCentro.Id, "Qualidade e Frescor", "Frutas/Legumes", null, TipoAlvo.Servico, 4);

        // 5. Vincular Alvos aos Dispositivos específicos
        // Açougue
        deviceAcougue.Alvos.Add(funcA1);
        deviceAcougue.Alvos.Add(funcA2);
        deviceAcougue.Alvos.Add(funcA3);
        deviceAcougue.Alvos.Add(limpezaAcougue);

        // Hortifrúti
        deviceHorti.Alvos.Add(funcH1);
        deviceHorti.Alvos.Add(funcH2);
        deviceHorti.Alvos.Add(funcH3);
        deviceHorti.Alvos.Add(qualidadeFrutas);

        await context.AlvosAvaliacao.AddRangeAsync(funcA1, funcA2, funcA3, limpezaAcougue, funcH1, funcH2, funcH3, qualidadeFrutas);
        await context.SaveChangesAsync();

        return Ok("Base de testes atualizada com sucesso!");
    }
}