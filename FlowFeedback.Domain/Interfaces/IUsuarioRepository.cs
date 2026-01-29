using FlowFeedback.Domain.Entities;

namespace FlowFeedback.Domain.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario?> ObterPorEmailAsync(string email);
    Task<Usuario?> ObterPorIdAsync(Guid id);
}