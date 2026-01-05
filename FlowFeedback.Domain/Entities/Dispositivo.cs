namespace FlowFeedback.Domain.Entities;

public class Dispositivo
{
    public string Id { get; private set; }
    public Guid TenantId { get; private set; }
    public Guid UnidadeId { get; private set; }
    public string NomeLocal { get; private set; }
    public bool Ativo { get; private set; }

    public ICollection<AlvoAvaliacao> Alvos { get; private set; } = new List<AlvoAvaliacao>();

    protected Dispositivo() { }

    public Dispositivo(string id, Guid tenantId, Guid unidadeId, string nomeLocal)
    {
        Id = id;
        TenantId = tenantId;
        UnidadeId = unidadeId;
        NomeLocal = nomeLocal;
        Ativo = true;
    }

    public void Desativar() => Ativo = false;
    public void Ativar() => Ativo = true;
}