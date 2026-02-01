using FlowFeedback.Domain.Enums;

namespace FlowFeedback.Domain.Entities
{
    public class UserTenant
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid TenantId { get; set; }
        public EnumUserRole Role { get; set; }
        public bool IsActive { get; set; }
    }
}
