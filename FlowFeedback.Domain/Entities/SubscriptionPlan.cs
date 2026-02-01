namespace FlowFeedback.Domain.Entities;

public class SubscriptionPlan
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? MaxStorageGB { get; set; }
    public int? MaxUsers { get; set; }
}
