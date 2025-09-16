using Agendamento.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Agendamento.Controllers
{
    [AllowAnonymous]
    public class HorarioAgendaController : BaseController
    {
        private readonly IHorarioAgendaAppService _horarioAgendaAppService;

        public HorarioAgendaController(
            IHorarioAgendaAppService horarioAgendaAppService)
        {
            _horarioAgendaAppService = horarioAgendaAppService;
        }

        [HttpGet]
        public async Task<IActionResult> BuscarHorariosDisponiveis(Guid idAtendente, Guid idServico, DateOnly data)
        {
            var retorno = await _horarioAgendaAppService.ListarHorariosDisponiveis(idAtendente, idServico, data);
            return Json(retorno);
        }
    }
}