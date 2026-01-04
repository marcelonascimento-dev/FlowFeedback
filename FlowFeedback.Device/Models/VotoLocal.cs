using SQLite;

namespace FlowFeedback.Device.Models;

public class VotoLocal
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public Guid AlvoId { get; set; }
    public int Nota { get; set; }
    public DateTime DataHora { get; set; }
    public string TagMotivo { get; set; }
    public bool Sincronizado { get; set; }
}