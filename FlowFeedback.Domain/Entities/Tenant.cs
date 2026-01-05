namespace FlowFeedback.Domain.Entities;

public class Tenant
{
    public Guid Id { get; private set; }
    public string NomeCorporativo { get; private set; }
    public string Cnpj { get; private set; }
    public bool Ativo { get; private set; }
    public string? LogoUrl { get; set; }
    public string? CorPrimaria { get; set; } // Ex: #27AE60
    public string? CorSecundaria { get; set; }

    protected Tenant() { }

    public Tenant(string nomeCorporativo, string cnpj)
    {
        Id = Guid.NewGuid();
        NomeCorporativo = nomeCorporativo;
        Cnpj = cnpj;
        Ativo = true;
    }
}