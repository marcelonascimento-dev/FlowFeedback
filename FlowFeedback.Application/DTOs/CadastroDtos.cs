using FlowFeedback.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace FlowFeedback.Application.DTOs;

public record CreateTenantDto([Required] string NomeCorporativo, [Required] string Cnpj, string? LogoUrl, string? CorPrimaria, string? CorSecundaria);

public record CreateUnidadeDto([Required] Guid TenantId, [Required] string Nome, [Required] string Cidade, [Required] string Endereco, string? LogoUrlOverride, string? CorPrimariaOverride, string? CorSecundariaOverride);

public record CreateDispositivoDto([Required] Guid UnidadeId,[Required] Guid TenantId,[Required] string Identificador, [Required] string Nome);

public record CreateAlvoAvaliacaoDto([Required] Guid UnidadeId, [Required] TipoAlvo Tipo, [Required] string Titulo, string Subtitulo, string? ImagemUrl, int? Ordem);