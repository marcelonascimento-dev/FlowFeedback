namespace FlowFeedback.Application.Interfaces;

public interface ISetupService
{
    Task<bool> CreateInitialAdminAsync(string email, string password);
}
