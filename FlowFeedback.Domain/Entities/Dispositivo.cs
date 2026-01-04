namespace FlowFeedback.Domain.Entities;

public class Dispositivo
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public Guid UnidadeId { get; private set; }
    public string NomeLocal { get; private set; }
    public bool Ativo { get; private set; }

    protected Dispositivo() { }

    public Dispositivo(Guid id, Guid tenantId, Guid unidadeId, string nomeLocal)
    {
        Id = id;
        TenantId = tenantId;
        UnidadeId = unidadeId;
        NomeLocal = nomeLocal;
        Ativo = true;
    }
}