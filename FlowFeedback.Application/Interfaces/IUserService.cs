using FlowFeedback.Application.DTOs;

namespace FlowFeedback.Application.Interfaces;

public interface IUserService
{
    Task<UserResponseDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<UserResponseDto>> GetAllAsync();
    Task<UserResponseDto> CreateAsync(UserRequest request);
    Task UpdateAsync(Guid id, UserUpdateRequest request);
    Task DeleteAsync(Guid id);
}
