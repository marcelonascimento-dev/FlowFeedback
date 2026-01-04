namespace FlowFeedback.Application.DTOs;

public record ResumoNpsDto(
    string Nome,
    int TotalVotos,
    double MediaNota,
    double Nps,
    int Promotores,
    int Neutros,
    int Detratores
);