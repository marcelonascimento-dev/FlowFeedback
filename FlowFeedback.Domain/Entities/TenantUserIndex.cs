namespace FlowFeedback.Domain.Entities
{
    public sealed class TenantUserIndex
    {
        public Guid UserId { get; init; }
        public long TenantCodigo { get; init; }
        public bool Ativo { get; init; }
    }

}
