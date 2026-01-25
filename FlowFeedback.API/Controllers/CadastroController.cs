using FlowFeedback.Application.DTOs;
using FlowFeedback.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FlowFeedback.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CadastroController : ControllerBase
{
    private readonly ICadastroService _cadastroService;

    public CadastroController(ICadastroService cadastroService)
    {
        _cadastroService = cadastroService;
    }

    [HttpPost("tenant")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CadastrarTenant([FromBody] CreateTenantDto dto)
    {
        var result = await _cadastroService.CadastrarTenantAsync(dto);
        return CreatedAtAction(nameof(CadastrarTenant), new { id = result.Id }, result);
    }

    [HttpPost("unidade")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CadastrarUnidade([FromBody] CreateUnidadeDto dto)
    {
        try
        {
            var result = await _cadastroService.CadastrarUnidadeAsync(dto);
            return CreatedAtAction(nameof(CadastrarUnidade), new { id = result.Id }, result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { erro = ex.Message });
        }
    }

    [HttpPost("dispositivo")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CadastrarDispositivo([FromBody] CreateDispositivoDto dto)
    {
        try
        {
            var result = await _cadastroService.CadastrarDispositivoAsync(dto);
            return CreatedAtAction(nameof(CadastrarDispositivo), new { id = result.Id }, result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { erro = ex.Message });
        }
    }

    [HttpPost("alvo-avaliacao")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CadastrarAlvo([FromBody] CreateAlvoAvaliacaoDto dto)
    {
        try
        {
            var result = await _cadastroService.CadastrarAlvoAsync(dto);
            return CreatedAtAction(nameof(CadastrarAlvo), new { id = result.Id }, result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { erro = ex.Message });
        }
    }

    [HttpPost("alvo-dispositivo")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CadastrarAlvoDispositivo([FromBody] CreateAlvoDispositivoDto dto)
    {
        try
        {
            await _cadastroService.VincularAlvosDispositivoAsync(dto);
            return CreatedAtAction(nameof(CadastrarAlvoDispositivo), new { dispositivoId = dto.DispositivoId }, null);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { erro = ex.Message });
        }
    }
}