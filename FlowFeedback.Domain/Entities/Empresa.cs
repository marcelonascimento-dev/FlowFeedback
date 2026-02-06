namespace FlowFeedback.Domain.Entities;

public class Empresa
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string CNPJ { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;

    // Endereço
    public string CEP { get; set; } = string.Empty;
    public string Logradouro { get; set; } = string.Empty;
    public string Numero { get; set; } = string.Empty;
    public string? Complemento { get; set; }
    public string Bairro { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;
    public string UF { get; set; } = string.Empty;

    // Customização
    public string? LogoUrlOverride { get; set; }
    public string? CorPrimariaOverride { get; set; }
    public string? CorSecundariaOverride { get; set; }

    public bool Ativo { get; set; }
}