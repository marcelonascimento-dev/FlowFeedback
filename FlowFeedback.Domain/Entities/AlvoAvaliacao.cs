using FlowFeedback.Domain.Enums;

namespace FlowFeedback.Domain.Entities;

public class AlvoAvaliacao
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public string Titulo { get; private set; }
    public string Subtitulo { get; private set; }
    public string ImagemUrl { get; private set; }
    public TipoAlvo Tipo { get; private set; }
    public bool Ativo { get; private set; }
    public int OrdemExibicao { get; private set; }

    public AlvoAvaliacao(Guid tenantId, string titulo, string subtitulo, string imagemUrl, TipoAlvo tipo, int ordemExibicao)
    {
        Id = Guid.NewGuid();
        TenantId = tenantId;
        Titulo = titulo;
        Subtitulo = subtitulo;
        ImagemUrl = imagemUrl;
        Tipo = tipo;
        OrdemExibicao = ordemExibicao;
        Ativo = true;
    }

    public void Desativar() => Ativo = false;
    public void AtualizarImagem(string novaUrl) => ImagemUrl = novaUrl;
}
