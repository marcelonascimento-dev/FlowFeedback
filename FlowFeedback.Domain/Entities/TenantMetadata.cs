namespace FlowFeedback.Domain.Entities;

public class TenantMetadata
{
    public int Id { get; set; }
    public string DbServer { get; set; }
    public string DbName { get; set; }
    public string DbUser { get; set; }
    public string DbPasswordEncrypted { get; set; }
}