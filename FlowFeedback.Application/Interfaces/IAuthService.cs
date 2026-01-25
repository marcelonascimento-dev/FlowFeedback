namespace FlowFeedback.Application.Interfaces;

public interface IAuthService
{
    Task<string?> AutenticarAsync(string email, string senha);
    string HashSenha(string senha);
}