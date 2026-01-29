using FlowFeedback.Domain.Enums;

namespace FlowFeedback.Domain.Entities;

public class Tenant
{
    public Guid Id { get; set; }
    public long Codigo { get; set; }
    public string Nome { get; set; }
    public string Slug { get; set; }
    public EnumTipoAmbiente TipoAmbiente { get; set; }
    public string ConnectionSecretKey { get; set; }
    public EnumStatusCadastro Status { get; set; }
    public DateTime DataCriacao { get; set; }
}