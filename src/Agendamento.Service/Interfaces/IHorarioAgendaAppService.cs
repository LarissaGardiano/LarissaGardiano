
namespace Agendamento.Service.Interfaces
{
    public interface IHorarioAgendaAppService
    {
        Task<IEnumerable<TimeSpan>> ListarHorariosDisponiveis(Guid idAtendente, Guid idServico, DateOnly data);

        Task<bool> VerificarDisponibilidade(Guid idAtendente, DateTime data);
    }
}