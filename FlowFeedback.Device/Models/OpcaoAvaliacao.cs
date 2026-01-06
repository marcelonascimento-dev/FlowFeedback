using FlowFeedback.Domain.Enums;

namespace FlowFeedback.Device.Models;

public class OpcaoAvaliacao
{
    public NivelSatisfacao Nivel { get; set; }
    public int Valor => (int)Nivel;
    public string Emoji { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public Color Cor { get; set; } = Colors.Gray;
    public string ImageSource { get; set; } = string.Empty;
}