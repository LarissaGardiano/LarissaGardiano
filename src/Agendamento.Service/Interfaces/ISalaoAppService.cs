using Agendamento.Infra.CrossCutting.Util;
using Agendamento.Service.Entidades;

namespace Agendamento.Service.Interfaces
{
    public interface ISalaoAppService
    {
        Task<Salao> Consultar(string nomeSalao);

        Task<Salao> Consultar(Guid idSalao);

        Task<RetornoGenerico> Alterar(Salao obj);
    }
}