namespace FlowFeedback.Domain.Entities;

public class Empresa
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public Tenant Tenant { get; set; }
    public string Nome { get; set; }

    // Novos campos de Localização e Override
    public string Cidade { get; set; }
    public string Endereco { get; set; }
    public string? LogoUrlOverride { get; set; }
    public string? CorPrimariaOverride { get; set; }
    public string? CorSecundariaOverride { get; set; }

    public bool Ativo { get; set; }
}