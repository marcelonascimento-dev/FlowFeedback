using FlowFeedback.Application.Events;
using FlowFeedback.Application.Interfaces;
using MassTransit;

namespace FlowFeedback.API.Consumers;

public class ProcessarVotosConsumer(IFeedbackService FeedbackService, ILogger<ProcessarVotosConsumer> Logger) : IConsumer<PacoteVotosRecebidoEvent>
{
    public async Task Consume(ConsumeContext<PacoteVotosRecebidoEvent> context)
    {
        try
        {
            Logger.LogInformation("Iniciando processamento assíncrono do pacote recebido em {Data}", context.Message.DataIngestao);

            await FeedbackService.ProcessarPacoteVotos(context.Message.Dados);

            Logger.LogInformation("Pacote processado com sucesso.");
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Erro ao processar votos. A mensagem irá para a DLQ (Dead Letter Queue).");
            throw;
        }
    }
}