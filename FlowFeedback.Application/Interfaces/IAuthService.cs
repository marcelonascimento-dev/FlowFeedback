using FlowFeedback.Application.Services;

namespace FlowFeedback.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponse?> AutenticarAsync(string email, string senha);
    string HashSenha(string senha);
}