using System.ComponentModel.DataAnnotations;

namespace FlowFeedback.Domain.Entities;

public class Dispositivo
{
    public Guid Id { get; set; }

    [Required]
    public Guid EmpresaId { get; set; }

    [Required]
    public Guid TenantId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Nome { get; set; }

    [Required]
    [MaxLength(100)]
    public string Identificador { get; set; }

    public bool Ativo { get; set; } = true;

    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

    public List<AlvoAvaliacao> AlvosAvaliacao { get; set; } = [];
}