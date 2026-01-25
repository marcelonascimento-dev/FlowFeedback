using FlowFeedback.Application.DTOs;
using FlowFeedback.Application.Interfaces;
using FlowFeedback.Domain.Entities;
using FlowFeedback.Domain.Interfaces;

namespace FlowFeedback.Application.Services;

public class CadastroService(ICadastroRepository repository, IDispositivoRepository dispositivoRepository) : ICadastroService
{
    public async Task<TenantSaidaDto> CadastrarTenantAsync(CreateTenantDto dto)
    {
        var tenant = new Tenant
        {
            Id = Guid.NewGuid(),
            Nome = dto.NomeCorporativo,
            Cnpj = dto.Cnpj,
            LogoUrl = dto.LogoUrl,
            CorPrimaria = dto.CorPrimaria,
            CorSecundaria = dto.CorSecundaria,
            Ativo = true,
            DataCriacao = DateTime.UtcNow
        };

        await repository.AddTenantAsync(tenant);
        return new TenantSaidaDto(tenant.Id, tenant.Nome, tenant.Cnpj);
    }

    public async Task<UnidadeSaidaDto> CadastrarUnidadeAsync(CreateUnidadeDto dto)
    {
        var tenant = await repository.GetTenantByIdAsync(dto.TenantId);
        if (tenant == null) throw new KeyNotFoundException("Tenant não encontrado.");

        var unidade = new Unidade
        {
            Id = Guid.NewGuid(),
            TenantId = dto.TenantId,
            Nome = dto.Nome,
            Cidade = dto.Cidade,
            Endereco = dto.Endereco,
            LogoUrlOverride = dto.LogoUrlOverride,
            CorPrimariaOverride = dto.CorPrimariaOverride,
            CorSecundariaOverride = dto.CorSecundariaOverride,
            Ativo = true
        };

        await repository.AddUnidadeAsync(unidade);
        return new UnidadeSaidaDto(unidade.Id, unidade.TenantId, unidade.Nome);
    }

    public async Task<DispositivoSaidaDto> CadastrarDispositivoAsync(CreateDispositivoDto dto)
    {
        var unidade = await repository.GetUnidadeByIdAsync(dto.UnidadeId);
        if (unidade == null) throw new KeyNotFoundException("Unidade não encontrada.");

        if (unidade.TenantId != dto.TenantId)
            throw new InvalidOperationException("A Unidade informada não pertence ao Tenant informado.");

        if (await dispositivoRepository.DispositivoExisteAsync(dto.Identificador))
            throw new InvalidOperationException($"O Identificador '{dto.Identificador}' já está em uso.");

        var dispositivo = new Dispositivo
        {
            Id = Guid.NewGuid(),
            UnidadeId = dto.UnidadeId,
            TenantId = dto.TenantId,
            Nome = dto.Nome,
            Identificador = dto.Identificador,
            Ativo = true,
            DataCriacao = DateTime.UtcNow
        };

        await dispositivoRepository.AddDispositivoAsync(dispositivo);
        return new DispositivoSaidaDto(dispositivo.Id, dispositivo.UnidadeId, dispositivo.Nome, dispositivo.Identificador);
    }

    public async Task<AlvoAvaliacaoSaidaDto> CadastrarAlvoAsync(CreateAlvoAvaliacaoDto dto)
    {
        var unidade = await repository.GetUnidadeByIdAsync(dto.UnidadeId);
        if (unidade == null) throw new KeyNotFoundException("Unidade não encontrada.");

        var alvo = new AlvoAvaliacao
        {
            Id = Guid.NewGuid(),
            UnidadeId = dto.UnidadeId,
            TenantId = unidade.TenantId,
            Nome = dto.Titulo, 
            Subtitulo = dto.Subtitulo,
            ImagemUrl = dto.ImagemUrl,
            Ordem = dto.Ordem ?? 0,
            Tipo = dto.Tipo,
            Ativo = true
        };

        await repository.AddAlvoAsync(alvo);
        return new AlvoAvaliacaoSaidaDto(alvo.Id, alvo.Nome, alvo.Tipo.ToString());
    }

    public async Task VincularAlvosDispositivoAsync(CreateAlvoDispositivoDto dto)
    {
        if (!Guid.TryParse(dto.DispositivoId, out var devId))
            throw new ArgumentException("ID do dispositivo inválido.");

        var dispositivo = await dispositivoRepository.GetDispositivoWithAlvosAsync(devId);

        if (dispositivo == null)
            throw new KeyNotFoundException("Dispositivo não encontrado.");

        dispositivo.AlvosAvaliacao.Clear();

        foreach (var idString in dto.alvosId)
        {
            if (Guid.TryParse(idString, out var alvoId))
            {
                var alvo = await repository.GetAlvoByIdAsync(alvoId);
                if (alvo != null && alvo.TenantId == dispositivo.TenantId)
                {
                    dispositivo.AlvosAvaliacao.Add(alvo);
                }
            }
        }

        await dispositivoRepository.UpdateDispositivoAsync(dispositivo);
    }
}