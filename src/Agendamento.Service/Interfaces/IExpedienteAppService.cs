using Agendamento.Infra.CrossCutting.Util;
using Agendamento.Service.Entidades;

namespace Agendamento.Service.Interfaces
{
    public interface IExpedienteAppService
    {
        Task<Expediente> ConsultarPorAtendente(Guid id);

        Task<RetornoGenerico> Alterar(Expediente obj);
    }
}