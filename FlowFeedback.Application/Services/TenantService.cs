using FlowFeedback.Application.DTOs;
using FlowFeedback.Application.Interfaces;
using FlowFeedback.Application.Models;
using FlowFeedback.Domain.Entities;
using FlowFeedback.Domain.Enums;
using FlowFeedback.Domain.Interfaces;
using FlowFeedback.Domain.Models;

namespace FlowFeedback.Application.Services;

public sealed class TenantService(ITenantRepository tenantRepository) : ITenantService
{
    public async Task<(Guid, long, DateTime)> CadastrarTenantAsync(TenantCreateDto dto)
    {
        if (dto is null)
            throw new ArgumentNullException(nameof(dto));

        if (string.IsNullOrWhiteSpace(dto.Nome))
            throw new ArgumentException("Nome do tenant é obrigatório.");

        if (string.IsNullOrWhiteSpace(dto.Slug))
            throw new ArgumentException("Slug do tenant é obrigatório.");

        var tenant = new Tenant
        {
            Id = Guid.NewGuid(),
            Nome = dto.Nome.Trim(),
            Slug = dto.Slug.Trim().ToLowerInvariant(),
            Status = EnumStatusCadastro.Ativo,
            TipoAmbiente = dto.TipoAmbiente,
            ConnectionSecretKey = dto.ConnectionSecretKey
        };

        var result = await tenantRepository.CadastrarTenantAsync(tenant);

        return (result.Id, result.Codigo, result.DataCriacao);
    }

    public async Task<TenantResponseDto> GetTenantAsync(Guid tenantId)
    {
        if (tenantId == Guid.Empty)
            throw new ArgumentException("TenantId inválido.");

        var tenant = await tenantRepository.GetTenantAsync(tenantId);

        if (tenant is null)
            throw new KeyNotFoundException("Tenant não encontrado.");

        return MapToDto(tenant);
    }

    public async Task<TenantResponseDto> GetTenantAsync(string slug)
    {
        if (string.IsNullOrWhiteSpace(slug))
            throw new ArgumentException("Slug inválido.");

        var tenant = await tenantRepository.GetTenantAsync(slug.Trim().ToLowerInvariant());

        if (tenant is null)
            throw new KeyNotFoundException("Tenant não encontrado.");

        return MapToDto(tenant);
    }

    public async Task<TenantResponseDto> GetTenantAsync(long codigo)
    {
        if (codigo <= 0)
            throw new ArgumentException("Código do tenant inválido.");

        var tenant = await tenantRepository.GetTenantAsync(codigo);

        return tenant is null ? throw new KeyNotFoundException("Tenant não encontrado.") : MapToDto(tenant);
    }

    private static TenantResponseDto MapToDto(Tenant tenant)
    {
        return new TenantResponseDto
        {
            Id = tenant.Id,
            Codigo = tenant.Codigo,
            Nome = tenant.Nome,
            Slug = tenant.Slug,
            Status = tenant.Status,
            TipoAmbiente = tenant.TipoAmbiente,
            DataCriacao = tenant.DataCriacao
        };
    }
}
