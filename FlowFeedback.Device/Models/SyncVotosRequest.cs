using System;
using System.Collections.Generic;
using System.Text;

namespace FlowFeedback.Device.Models;

public record SyncVotosRequest(
    Guid TenantId,
    Guid DeviceId,
    List<VotoItemDto> Votos);
