using FlowFeedback.Domain.Entities;

namespace FlowFeedback.Domain.Repositories;

public interface IUsuarioRepository
{
    Task<Usuario?> ObterPorEmailAsync(string email);
}