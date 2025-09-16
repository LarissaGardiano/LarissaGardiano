using Agendamento.Infra.CrossCutting.Util;
using Agendamento.Service.Entidades;

namespace Agendamento.Service.Interfaces
{
    public interface IServicoAppService
    {
        Task<Servico> Consultar(Guid id);

        Task<IEnumerable<Servico>> Listar(Guid id);

        Task<IEnumerable<Servico>> ListarAtivos(Guid id);

        Task<RetornoGenerico> Incluir(Servico obj);

        Task<RetornoGenerico> Alterar(Servico obj);
    }
}