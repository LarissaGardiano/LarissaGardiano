using Agendamento.Infra.CrossCutting.Util;
using Agendamento.Infra.CrossCutting.Util.ViewModel;
using Agendamento.Service.Entidades;
using System.Security.Claims;

namespace Agendamento.Service.Interfaces
{
    public interface IAtendenteAppService
    {
        Task<Atendente> Consultar(Guid id);

        Task<Atendente> ConsultarPorEmail(string email);

        Task<IEnumerable<Atendente>> Listar(Guid idSalao);

        Task<RetornoGenerico> Incluir(Atendente obj, string salao);

        Task<RetornoGenerico> Alterar(Atendente obj);

        Task<DadosAtendenteLogadoViewModel> Autenticar(LoginViewModel obj);

        ClaimsPrincipal CriarClaims(DadosAtendenteLogadoViewModel obj);
    }
}