using FlowFeedback.Domain.Enums;

namespace FlowFeedback.Domain.Models
{
    public class TenantDto
    {
        public int Codigo { get; set; }
        public required string Nome { get; set; }
        public required string Slug { get; set; }
        public EnumTipoAmbiente TipoAmbiente { get; set; }
        public required string ConnectionSecretKey { get; set; }
    }

}
