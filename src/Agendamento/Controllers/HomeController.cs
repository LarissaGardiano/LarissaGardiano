using Agendamento.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Newtonsoft.Json;
using Agendamento.Service.Entidades;
using Microsoft.AspNetCore.Authorization;
using Agendamento.Infra.CrossCutting.Util.Settings;
using Microsoft.Extensions.Options;

namespace Agendamento.Controllers
{
    [AllowAnonymous]
    public class HomeController : BaseController
    {
        private readonly WebSettings _webSettings;
        private readonly ILogger<HomeController> _logger;
        private readonly ISalaoAppService _salaoAppService;
        private readonly IServicoAppService _servicoAppService;

        public HomeController(
            IOptions<WebSettings> options,
            ILogger<HomeController> logger,
            ISalaoAppService salaoAppService,
            IServicoAppService servicoAppService)
        {
            _webSettings = options.Value;
            _logger = logger;
            _salaoAppService = salaoAppService;
            _servicoAppService = servicoAppService;
        }

        [HttpGet]
        public async Task<IActionResult> Menu()
        {
            var salao = await _salaoAppService.Consultar(NomeSalaoSelecionado);

            if (salao is null)
                return RedirectToAction("Menu", "Home");

            ViewBag.Servicos = await _servicoAppService.ListarAtivos(salao.IdSalao);
            return View();
        }

        private Claim CarregarInformacoesDoSalao(Salao salao)
        {
            return new Claim(ClaimTypes.Actor, JsonConvert.SerializeObject(salao));
        }
    }
}