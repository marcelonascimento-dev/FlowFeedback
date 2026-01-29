namespace FlowFeedback.Application.Interfaces.Security;

public interface ISecretProvider
{
    Task<string> GetSecretAsync(string secretKey);
}
