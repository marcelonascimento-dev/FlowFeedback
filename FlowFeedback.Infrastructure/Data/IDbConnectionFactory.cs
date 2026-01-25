using System.Data;

namespace FlowFeedback.Infrastructure.Data;

public interface IDbConnectionFactory
{
    IDbConnection CreateMasterConnection();
    IDbConnection CreateTenantConnection();
}