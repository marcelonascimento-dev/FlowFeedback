using FlowFeedback.Application.DTOs;
using FlowFeedback.Domain.Entities;

namespace FlowFeedback.Application.Mappings;

public static class EmpresaMappingExtensions
{
    public static Empresa ToEntity(this EmpresaCreateDto dto)
    {
        return new Empresa
        {
            Id = Guid.NewGuid(),
            Nome = dto.Nome,
            CNPJ = dto.CNPJ,
            Email = dto.Email,
            Telefone = dto.Telefone,
            CEP = dto.CEP,
            Logradouro = dto.Logradouro,
            Numero = dto.Numero,
            Complemento = dto.Complemento,
            Bairro = dto.Bairro,
            Cidade = dto.Cidade,
            UF = dto.UF,
            LogoUrlOverride = dto.LogoUrlOverride,
            CorPrimariaOverride = dto.CorPrimariaOverride,
            CorSecundariaOverride = dto.CorSecundariaOverride,
            Ativo = true
        };
    }

    public static void UpdateEntity(this EmpresaUpdateDto dto, Empresa entity)
    {
        entity.Nome = dto.Nome;
        entity.CNPJ = dto.CNPJ;
        entity.Email = dto.Email;
        entity.Telefone = dto.Telefone;
        entity.CEP = dto.CEP;
        entity.Logradouro = dto.Logradouro;
        entity.Numero = dto.Numero;
        entity.Complemento = dto.Complemento;
        entity.Bairro = dto.Bairro;
        entity.Cidade = dto.Cidade;
        entity.UF = dto.UF;
        entity.LogoUrlOverride = dto.LogoUrlOverride;
        entity.CorPrimariaOverride = dto.CorPrimariaOverride;
        entity.CorSecundariaOverride = dto.CorSecundariaOverride;
        entity.Ativo = dto.Ativo;
    }

    public static EmpresaResponseDto ToResponseDto(this Empresa entity)
    {
        return new EmpresaResponseDto(
            entity.Id,
            entity.Nome,
            entity.CNPJ,
            entity.Email,
            entity.Telefone,
            entity.CEP,
            entity.Logradouro,
            entity.Numero,
            entity.Complemento,
            entity.Bairro,
            entity.Cidade,
            entity.UF,
            entity.LogoUrlOverride,
            entity.CorPrimariaOverride,
            entity.CorSecundariaOverride,
            entity.Ativo
        );
    }
}
