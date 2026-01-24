using FlowFeedback.Application.DTOs;

namespace FlowFeedback.Application.Interfaces;

public interface IFeedbackService
{
    Task ProcessarPacoteVotos(PacoteVotosDto pacote);
}