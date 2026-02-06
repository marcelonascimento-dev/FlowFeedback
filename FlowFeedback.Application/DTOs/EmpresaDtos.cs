namespace FlowFeedback.Application.DTOs;

public record EmpresaCreateDto(
    string Nome,
    string CNPJ,
    string Email,
    string Telefone,
    string CEP,
    string Logradouro,
    string Numero,
    string? Complemento,
    string Bairro,
    string Cidade,
    string UF,
    string? LogoUrlOverride = null,
    string? CorPrimariaOverride = null,
    string? CorSecundariaOverride = null
);

public record EmpresaUpdateDto(
    Guid Id,
    string Nome,
    string CNPJ,
    string Email,
    string Telefone,
    string CEP,
    string Logradouro,
    string Numero,
    string? Complemento,
    string Bairro,
    string Cidade,
    string UF,
    string? LogoUrlOverride = null,
    string? CorPrimariaOverride = null,
    string? CorSecundariaOverride = null,
    bool Ativo = true
);

public record EmpresaResponseDto(
    Guid Id,
    string Nome,
    string CNPJ,
    string Email,
    string Telefone,
    string CEP,
    string Logradouro,
    string Numero,
    string? Complemento,
    string Bairro,
    string Cidade,
    string UF,
    string? LogoUrlOverride,
    string? CorPrimariaOverride,
    string? CorSecundariaOverride,
    bool Ativo
);
