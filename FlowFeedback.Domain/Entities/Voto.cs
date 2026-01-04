using FlowFeedback.Domain.Enums;

namespace FlowFeedback.Domain.Entities;

public class Voto
{
    public long Id { get; private set; }
    public Guid TenantId { get; private set; }
    public Guid DeviceId { get; private set; }
    public Guid AlvoAvaliacaoId { get; private set; }

    public int Nota { get; private set; }
    public string? TagMotivo { get; private set; }

    public DateTime DataHoraVoto { get; private set; }
    public DateTime DataHoraSincronizacao { get; private set; }

    public ClassificacaoNps Classificacao => Nota switch
    {
        >= 9 => ClassificacaoNps.Promotor,
        >= 7 => ClassificacaoNps.Neutro,
        _ => ClassificacaoNps.Detrator
    };

    protected Voto() { }

    public Voto(Guid tenantId, Guid deviceId, Guid alvoAvaliacaoId, int nota, DateTime dataHoraVoto, string? tagMotivo = null)
    {
        if (tenantId == Guid.Empty)
            throw new ArgumentException("TenantId inválido.", nameof(tenantId));

        if (nota < 0 || nota > 10)
            throw new ArgumentOutOfRangeException(nameof(nota), "A nota NPS deve ser entre 0 e 10.");

        TenantId = tenantId;
        DeviceId = deviceId;
        AlvoAvaliacaoId = alvoAvaliacaoId;
        Nota = nota;
        DataHoraVoto = dataHoraVoto;
        TagMotivo = tagMotivo;
        DataHoraSincronizacao = DateTime.UtcNow;
    }
}