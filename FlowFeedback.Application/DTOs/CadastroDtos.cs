using System.ComponentModel.DataAnnotations;
using FlowFeedback.Domain.Enums;

namespace FlowFeedback.Application.DTOs;

// DTOs estritos conforme solicitado
public record CreateTenantDto(
    [Required] string NomeCorporativo,
    [Required] string Cnpj,
    string? LogoUrl,
    string? CorPrimaria,
    string? CorSecundaria
);

public record CreateUnidadeDto(
    [Required] Guid TenantId,
    [Required] string Nome,
    [Required] string Cidade,
    [Required] string Endereco,
    string? LogoUrlOverride,
    string? CorPrimariaOverride,
    string? CorSecundariaOverride
);

public record CreateDispositivoDto(
    [Required] Guid UnidadeId,
    [Required] Guid TenantId,
    [Required] string Identificador,
    [Required] string Nome
);

public record CreateAlvoAvaliacaoDto(
    [Required] Guid UnidadeId,
    [Required] TipoAlvo Tipo,
    [Required] string Titulo,
    string Subtitulo,
    string? ImagemUrl,
    int? Ordem
);

public record CreateAlvoDispositivoDto(
    [Required] string DispositivoId,
    IEnumerable<string> alvosId
);

public record TenantSaidaDto(Guid Id, string Nome, string Cnpj);
public record UnidadeSaidaDto(Guid Id, Guid TenantId, string Nome);
public record DispositivoSaidaDto(Guid Id, Guid UnidadeId, string Nome, string Identificador);
public record AlvoAvaliacaoSaidaDto(Guid Id, string Titulo, string TipoDescricao);