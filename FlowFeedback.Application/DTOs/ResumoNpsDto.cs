namespace FlowFeedback.Application.DTOs;

public record ResumoNpsDto(
    string Nome,
    int TotalVotos,
    double MediaNota,
    double Mediana,
    double Nps,
    int Promotores,
    int Neutros,
    int Detratores,
    double PercentualConfianca
);