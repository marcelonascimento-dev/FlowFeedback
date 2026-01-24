using FlowFeedback.Application.DTOs;
using FlowFeedback.Application.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace FlowFeedback.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SyncController(IPublishEndpoint PublishEndpoint) : ControllerBase
{
    [HttpPost("votos")]
    public async Task<IActionResult> SincronizarVotos([FromBody] PacoteVotosDto pacote)
    {
        if (pacote == null || pacote.Votos.Count == 0)
            return BadRequest("Pacote inválido ou vazio.");

        var evento = new PacoteVotosRecebidoEvent
        {
            Dados = pacote
        };

        await PublishEndpoint.Publish(evento);

        return Accepted();
    }
}