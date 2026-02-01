using FlowFeedback.Domain.Enums;

namespace FlowFeedback.Domain.Entities;

public class Tenant
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string DbServer { get; set; } = string.Empty;
    public string DbName { get; set; } = string.Empty;
    public string DbUser { get; set; } = string.Empty;
    public byte[] DbPassword { get; set; } = Array.Empty<byte>();
    public EnumStatusCadastro Status { get; set; }
    public DateTime CreatedAt { get; set; }
}