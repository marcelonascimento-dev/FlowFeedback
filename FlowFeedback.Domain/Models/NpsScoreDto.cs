namespace FlowFeedback.Domain.Models;

public record NpsScoreDto
{
    public double Score { get; init; }
    public int TotalVotos { get; init; }
    public int QtdPromotores { get; init; }
    public int QtdNeutros { get; init; }
    public int QtdDetratores { get; init; }

    public double PercPromotores => TotalVotos == 0 ? 0 : (double)QtdPromotores / TotalVotos * 100;
    public double PercNeutros => TotalVotos == 0 ? 0 : (double)QtdNeutros / TotalVotos * 100;
    public double PercDetratores => TotalVotos == 0 ? 0 : (double)QtdDetratores / TotalVotos * 100;
}
