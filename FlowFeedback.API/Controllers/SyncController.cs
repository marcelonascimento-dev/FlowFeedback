using FlowFeedback.Application.DTOs;
using FlowFeedback.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FlowFeedback.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SyncController : ControllerBase
{
    private readonly IFeedbackService _feedbackService;

    public SyncController(IFeedbackService feedbackService)
    {
        _feedbackService = feedbackService;
    }

    [HttpPost("votos")]
    public async Task<IActionResult> SincronizarVotos([FromBody] PacoteVotosDto pacote)
    {
        if (pacote == null || pacote.Votos == null || !pacote.Votos.Any())
            return BadRequest("Pacote de votos inválido ou vazio.");

        await _feedbackService.ProcessarVotosDoTabletAsync(
            pacote.TenantId,
            pacote.DeviceId,
            pacote.Votos
        );

        return Ok(new { mensagem = "Sincronização concluída com sucesso", total = pacote.Votos.Count });
    }
}