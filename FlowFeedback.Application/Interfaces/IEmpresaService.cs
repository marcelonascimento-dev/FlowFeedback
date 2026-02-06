using FlowFeedback.Application.DTOs;

namespace FlowFeedback.Application.Interfaces;

public interface IEmpresaService
{
    Task<IEnumerable<EmpresaResponseDto>> GetAllAsync();
    Task<EmpresaResponseDto?> GetByIdAsync(Guid id);
    Task<EmpresaResponseDto> AddAsync(EmpresaCreateDto dto);
    Task UpdateAsync(EmpresaUpdateDto dto);
    Task DeleteAsync(Guid id);
}
