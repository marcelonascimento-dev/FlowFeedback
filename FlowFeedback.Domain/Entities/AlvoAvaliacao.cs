using FlowFeedback.Domain.Enums;

namespace FlowFeedback.Domain.Entities;

public class AlvoAvaliacao
{
    public Guid Id { get; set; }
    public Guid EmpresaId { get; set; }

    public string Nome { get; set; }
    public string? Subtitulo { get; set; }
    public string? ImagemUrl { get; set; }
    public int Ordem { get; set; }

    public TipoAlvo Tipo { get; set; }
    public bool Ativo { get; set; }

    // Relacionamento N:N com Dispositivos (precisa existir na base)
    public ICollection<Dispositivo> Dispositivos { get; set; } = [];
}