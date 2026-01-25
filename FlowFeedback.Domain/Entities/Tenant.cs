namespace FlowFeedback.Domain.Entities;

public class Tenant
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Cnpj { get; set; }

    public string? LogoUrl { get; set; }
    public string? CorPrimaria { get; set; }
    public string? CorSecundaria { get; set; }

    public bool Ativo { get; set; }
    public DateTime DataCriacao { get; set; }

    public ICollection<Unidade> Unidades { get; set; }
}