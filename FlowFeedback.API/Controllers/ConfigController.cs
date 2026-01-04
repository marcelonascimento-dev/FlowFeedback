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

    [HttpGet("{tenantId}")]
    public async Task<IActionResult> ObterConfiguracao(Guid tenantId)
    {
        var alvos = await _context.AlvosAvaliacao
            .AsNoTracking()
            .Where(a => a.TenantId == tenantId && a.Ativo)
            .OrderBy(a => a.OrdemExibicao)
            .ToListAsync();

        if (alvos.Count == 0)
            return NotFound(new { mensagem = "Nenhuma configuração encontrada." });

        var response = new
        {
            NomeEmpresa = "Grupo Max",
            Cards = alvos.Select(a => new
            {
                a.Id,
                a.Titulo,
                a.Subtitulo,
                a.ImagemUrl,
                Tipo = (int)a.Tipo
            })
        };

        return Ok(response);
    }
}