using Agendamento.Infra.CrossCutting.Util;
using Agendamento.Infra.CrossCutting.Util.Enums;
using Agendamento.Infra.CrossCutting.Util.ViewModel;

namespace Agendamento.Service.Interfaces
{
    public interface IAgendamentoAppService
    {
        Task<IEnumerable<MeuAgendamentoViewModel>> MeusAgendamentosPendentes(string telefone, Guid idSalao);

        Task<IEnumerable<MeuAgendamentoViewModel>> MeusAgendamentosHistorico(string telefone, Guid idSalao);

        Task<IEnumerable<OutroAgendamentoViewModel>> OutrosAgendamentos(Guid idAtendente, Guid idSalao, DateTime data);

        Task<RetornoGenerico> Cancelar(Guid idAgendamento);

        Task<RetornoGenerico> Finalizar(Guid idAgendamento);

        Task<RetornoGenerico> Incluir(Entidades.Agendamento obj);

        Task<IEnumerable<CalendarioAgendaViewModel>> CalendarioAgendaDoDia(Guid? idAtendente, DateTime? data, Guid? idAgendamento = null);

        Task<IEnumerable<Entidades.Agendamento>> ListarAgendamentosPorPeriodoEStatus(Guid idAtendente, DateTime dataInicio, DateTime dataTermino, StatusAgendamentoEnum status);
    }
}