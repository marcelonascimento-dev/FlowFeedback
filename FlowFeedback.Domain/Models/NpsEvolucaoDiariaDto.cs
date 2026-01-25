using System;
using System.Collections.Generic;
using System.Text;

namespace FlowFeedback.Domain.Models;

public record NpsEvolucaoDiariaDto(DateTime Data, double Score, int TotalVotos);
