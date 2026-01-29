using FlowFeedback.Domain.Enums;

namespace FlowFeedback.Domain.Models
{
    public class TenantDto
    {
        public int Codigo { get; set; }
        public string Nome { get; set; }
        public string Slug { get; set; }
        public EnumTipoAmbiente TipoAmbiente { get; set; }
        public string ConnectionSecretKey { get; set; }
    }
    
}
