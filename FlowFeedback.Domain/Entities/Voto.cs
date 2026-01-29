using FlowFeedback.Domain.Enums;

namespace FlowFeedback.Domain.Entities;

public class Voto
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public Guid EmpresaId { get; private set; }
    public string DeviceId { get; private set; }
    public Guid AlvoAvaliacaoId { get; private set; }
    public int Nota { get; private set; }
    public string? Comentario { get; private set; }
    public IEnumerable<string>? Tags { get; private set; }
    public DateTime DataHoraVoto { get; private set; }
    public DateTime DataHoraSincronizacao { get; private set; }

    public ClassificacaoNps Classificacao => Nota switch
    {
        >= 9 => ClassificacaoNps.Promotor,
        >= 7 => ClassificacaoNps.Neutro,
        _ => ClassificacaoNps.Detrator
    };

    public Voto(Guid id, Guid tenantId, Guid EmpresaId, string deviceId, Guid alvoAvaliacaoId, int nota, DateTime dataHoraVoto, IEnumerable<string>? tags = null, string? comentario = null)
    {
        if (tenantId == Guid.Empty) throw new ArgumentException("TenantId inválido");
        if (EmpresaId == Guid.Empty) throw new ArgumentException("EmpresaId inválida");
        if (nota < 0 || nota > 10) throw new ArgumentOutOfRangeException("Nota deve ser entre 0 e 10");

        Id = id;
        TenantId = tenantId;
        EmpresaId = EmpresaId;
        DeviceId = deviceId;
        AlvoAvaliacaoId = alvoAvaliacaoId;
        Nota = nota;
        DataHoraVoto = dataHoraVoto;
        Tags = tags;
        DataHoraSincronizacao = DateTime.UtcNow;
        Comentario = comentario;
    }
}