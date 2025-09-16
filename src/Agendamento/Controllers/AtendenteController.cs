using Agendamento.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Agendamento.Controllers
{
    [Authorize]
    public class AtendenteController : BaseController
    {
        private readonly IAtendenteAppService _atendenteAppService;

        public AtendenteController(
            IAtendenteAppService atendenteAppService)
        {
            _atendenteAppService = atendenteAppService;
        }

        [HttpGet]
        public async Task<IActionResult> Consultar(Guid id)
        {
            var retorno = await _atendenteAppService.Consultar(id);
            return Json(retorno);
        }

        [HttpGet]
        public async Task<IActionResult> ListarTodosAtendentes()
        {
            var retorno = await _atendenteAppService.Listar(IdSalaoSelecionado);
            retorno = retorno.OrderBy(x => x.Nome);

            return Json(retorno);
        }
    }
}