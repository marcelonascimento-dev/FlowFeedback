using FlowFeedback.Domain.Entities;

namespace FlowFeedback.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(Guid id);
    Task<User> CadastrarAsync(User user);
}