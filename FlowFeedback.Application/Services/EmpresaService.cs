using FlowFeedback.Application.DTOs;
using FlowFeedback.Application.Interfaces;
using FlowFeedback.Application.Mappings;
using FlowFeedback.Domain.Interfaces;

namespace FlowFeedback.Application.Services;

public class EmpresaService(IEmpresaRepository empresaRepository) : IEmpresaService
{
    public async Task<IEnumerable<EmpresaResponseDto>> GetAllAsync()
    {
        var empresas = await empresaRepository.GetAllAsync();
        return empresas.Select(e => e.ToResponseDto());
    }

    public async Task<EmpresaResponseDto?> GetByIdAsync(Guid id)
    {
        var empresa = await empresaRepository.GetByIdAsync(id);
        return empresa?.ToResponseDto();
    }

    public async Task<EmpresaResponseDto> AddAsync(EmpresaCreateDto dto)
    {
        var empresa = dto.ToEntity();
        await empresaRepository.AddAsync(empresa);
        return empresa.ToResponseDto();
    }

    public async Task UpdateAsync(EmpresaUpdateDto dto)
    {
        var empresa = await empresaRepository.GetByIdAsync(dto.Id);
        if (empresa == null) throw new KeyNotFoundException("Empresa não encontrada.");

        dto.UpdateEntity(empresa);
        await empresaRepository.UpdateAsync(empresa);
    }

    public async Task DeleteAsync(Guid id)
    {
        await empresaRepository.DeleteAsync(id);
    }
}
