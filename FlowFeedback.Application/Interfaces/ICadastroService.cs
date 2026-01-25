using FlowFeedback.Application.DTOs;
using FlowFeedback.Domain.Entities;

namespace FlowFeedback.Application.Interfaces;

public interface ICadastroService
{
    Task<TenantSaidaDto> CadastrarTenantAsync(CreateTenantDto dto);
    Task<UnidadeSaidaDto> CadastrarUnidadeAsync(CreateUnidadeDto dto);
    Task<DispositivoSaidaDto> CadastrarDispositivoAsync(CreateDispositivoDto dto);
    Task<AlvoAvaliacaoSaidaDto> CadastrarAlvoAsync(CreateAlvoAvaliacaoDto dto);
    Task VincularAlvosDispositivoAsync(CreateAlvoDispositivoDto dto);
}