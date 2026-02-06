using FlowFeedback.Application.DTOs;
using FlowFeedback.Application.Interfaces;
using FlowFeedback.Domain.Entities;
using FlowFeedback.Domain.Interfaces;

namespace FlowFeedback.Application.Services;

public sealed class UserService(IUserRepository userRepository) : IUserService
{
    public async Task<UserResponseDto?> GetByIdAsync(Guid id)
    {
        var user = await userRepository.GetByIdAsync(id);
        return user is null ? null : MapToDto(user);
    }

    public async Task<IEnumerable<UserResponseDto>> GetAllAsync()
    {
        var users = await userRepository.GetAllAsync();
        return users.Select(MapToDto);
    }

    public async Task<UserResponseDto> CreateAsync(UserRequest request)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            IsActive = request.IsActive,
            CreatedAt = DateTime.UtcNow,
            EmailConfirmed = false
        };

        await userRepository.CadastrarAsync(user);
        return MapToDto(user);
    }

    public async Task UpdateAsync(Guid id, UserUpdateRequest request)
    {
        var user = await userRepository.GetByIdAsync(id);
        if (user == null) return;

        user.Name = request.Name;
        user.Email = request.Email;
        user.IsActive = request.IsActive;

        if (!string.IsNullOrWhiteSpace(request.Password))
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        }

        await userRepository.UpdateAsync(user);
    }

    public async Task DeleteAsync(Guid id)
    {
        await userRepository.DeleteAsync(id);
    }

    private static UserResponseDto MapToDto(User user)
    {
        return new UserResponseDto(
            Id: user.Id,
            Name: user.Name,
            Email: user.Email,
            IsActive: user.IsActive,
            EmailConfirmed: user.EmailConfirmed,
            CreatedAt: user.CreatedAt
        );
    }
}
