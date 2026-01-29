using System;
using System.Collections.Generic;
using System.Text;
using FlowFeedback.Application.DTOs;
using FlowFeedback.Domain.Entities;

namespace FlowFeedback.Application.Mappings
{
    public static class VotoMappingExtensions
    {
        public static Voto ToEntity(this RegistrarVotoDto dto, Dispositivo dispositivo)
        {
            return new Voto(dto.Id, dispositivo.TenantId, dispositivo.EmpresaId, dispositivo.Identificador, dto.IdAlvoAvaliacao, dto.Valor, dto.DataHora, dto.Tags, dto.Comentario);
        }
    }
}
