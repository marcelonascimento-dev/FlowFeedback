namespace FlowFeedback.Domain.Entities;

public class Usuario
{
    public Guid Id { get; set; }
    public int TenantCode { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string SenhaHash { get; set; } = string.Empty;
    public string Role { get; set; } = "User";
    public bool Ativo { get; set; }
}