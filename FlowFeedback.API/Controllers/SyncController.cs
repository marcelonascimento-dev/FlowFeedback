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
    public async Task<IActionResult> ReceberVotos([FromBody] PacoteVotosDto pacote)
    {
        if (pacote.Votos == null || !pacote.Votos.Any())
            return BadRequest("O pacote de votos está vazio.");

        await _feedbackService.ProcessarVotosDoTabletAsync(
            pacote.TenantId,
            pacote.DeviceId,
            pacote.Votos
        );

        return Ok(new { mensagem = "Votos sincronizados com sucesso", qtd = pacote.Votos.Count });
    }
}