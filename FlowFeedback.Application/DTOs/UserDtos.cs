namespace FlowFeedback.Application.DTOs;

public record UserRequest(string Name, string Email, string Password, bool IsActive);
public record UserUpdateRequest(string Name, string Email, string? Password, bool IsActive);
public record UserResponseDto(Guid Id, string Name, string Email, bool IsActive, bool EmailConfirmed, DateTime CreatedAt);
