using FlowFeedback.Application.DTOs;
using FlowFeedback.Domain.Entities;

namespace FlowFeedback.Application.Interfaces;

public interface ICadastroService
{
    Task<Tenant> CadastrarTenantAsync(CreateTenantDto dto);
    Task<Unidade> CadastrarUnidadeAsync(CreateUnidadeDto dto);
    Task<Dispositivo> CadastrarDispositivoAsync(CreateDispositivoDto dto);
    Task<AlvoAvaliacao> CadastrarAlvoAvaliacaoAsync(CreateAlvoAvaliacaoDto dto);
    Task VincularAlvoAvaliacaoADispositivoAsync(string dispositivoId, IEnumerable<string> alvosId);
}