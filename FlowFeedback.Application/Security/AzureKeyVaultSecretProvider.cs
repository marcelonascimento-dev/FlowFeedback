using FlowFeedback.Application.Interfaces.Security;

namespace FlowFeedback.Infrastructure.Security;

public sealed class AzureKeyVaultSecretProvider : ISecretProvider
{
    public async Task<string> GetSecretAsync(string secretKey)
    {
        throw new NotImplementedException();
    }
}
