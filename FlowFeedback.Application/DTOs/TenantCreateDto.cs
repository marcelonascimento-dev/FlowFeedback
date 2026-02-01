using FlowFeedback.Domain.Enums;

namespace FlowFeedback.Application.DTOs
{
    public sealed class TenantCreateDto
    {
        public string Name { get; init; } = string.Empty;
        public string Slug { get; init; } = string.Empty;
        public EnumStatusCadastro Status { get; init; }
        public string DbServer { get; init; } = string.Empty;
        public string DbName { get; init; } = string.Empty;
        public string DbUser { get; init; } = string.Empty;
        public string DbPassword { get; init; } = string.Empty;
    }

}
