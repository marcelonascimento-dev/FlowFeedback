using FlowFeedback.Domain.Enums;

namespace FlowFeedback.Application.DTOs
{
    public sealed class TenantCreateDto
    {
        public string Nome { get; init; } = string.Empty;
        public string Slug { get; init; } = string.Empty;
        public EnumStatusCadastro Status { get; init; }
        public EnumTipoAmbiente TipoAmbiente { get; init; }
        public string ConnectionSecretKey { get; init; } = string.Empty;
    }

}
