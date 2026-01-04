namespace FlowFeedback.Domain.Entities;

public class Unidade
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public string Nome { get; private set; }
    public string CodigoExterno { get; private set; } // Ex: "LJ-01" (Integração ERP)
    public bool Ativa { get; private set; }

    protected Unidade() { }

    public Unidade(Guid tenantId, string nome, string codigoExterno)
    {
        Id = Guid.NewGuid();
        TenantId = tenantId;
        Nome = nome;
        CodigoExterno = codigoExterno;
        Ativa = true;
    }
}