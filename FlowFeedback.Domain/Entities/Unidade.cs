namespace FlowFeedback.Domain.Entities;

public class Unidade
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public string NomeLoja { get; private set; }
    public string Cidade { get; private set; }
    public string Endereco { get; private set; }
    public bool Ativa { get; private set; }
    public string? LogoUrlOverride { get; set; }
    public string? CorPrimariaOverride { get; set; }
    public string? CorSecundariaOverride { get; set; }

    protected Unidade() { }

    public Unidade(Guid tenantId, string nomeLoja, string cidade, string endereco)
    {
        Id = Guid.NewGuid();
        TenantId = tenantId;
        NomeLoja = nomeLoja;
        Cidade = cidade;
        Endereco = endereco;
        Ativa = true;
    }
}