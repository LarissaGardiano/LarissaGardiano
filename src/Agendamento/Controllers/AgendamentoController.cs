using Agendamento.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Agendamento.Controllers
{
    [AllowAnonymous]
    public class AgendamentoController : BaseController
    {
        private readonly IAgendamentoAppService _agendamentoAppService;
        private readonly IAtendenteAppService _atendenteAppService;
        private readonly IServicoAppService _servicoAppService;

        public AgendamentoController(
            IAgendamentoAppService agendamentoAppService,
            IAtendenteAppService atendenteAppService,
            IServicoAppService servicoAppService)
        {
            _agendamentoAppService = agendamentoAppService;
            _atendenteAppService = atendenteAppService;
            _servicoAppService = servicoAppService;
        }

        [HttpGet]
        public async Task<IActionResult> ArmazenarTelefone(string telefone)
        {
            if (!TelefoneSelecionado.Equals(telefone))
                await AdicionarClaim(CarregarInformacaoTelefoneCliente(telefone));

            return RedirectToAction("MeusAgendamentosPendentes");
        }

        [HttpGet]
        [Route("/meus-agendamentos-pendentes")]
        public async Task<IActionResult> MeusAgendamentosPendentes()
        {
            if (IdSalaoSelecionado == Guid.Empty)
                return RedirectToAction("Index", "Home");

            var retorno = await _agendamentoAppService.MeusAgendamentosPendentes(TelefoneSelecionado, IdSalaoSelecionado);
            return View("Index", retorno);
        }

        [HttpGet]
        [Route("/meus-agendamentos-historico")]
        public async Task<IActionResult> MeusAgendamentosHistorico()
        {
            if (IdSalaoSelecionado == Guid.Empty)
                return RedirectToAction("Index", "Home");

            var retorno = await _agendamentoAppService.MeusAgendamentosHistorico(TelefoneSelecionado, IdSalaoSelecionado);
            return View("Index", retorno);
        }

        [HttpGet]
        public async Task<IActionResult> Reserva(Guid idServico)
        {
            if (IdSalaoSelecionado == Guid.Empty)
                return RedirectToAction("Index", "Home");

            ViewBag.Servico = await _servicoAppService.Consultar(idServico);
            var atendentes = await _atendenteAppService.Listar(IdSalaoSelecionado);

            return View(atendentes);
        }

        [HttpPost]
        public async Task<IActionResult> Incluir([FromBody] Service.Entidades.Agendamento obj)
        {
            obj.IdSalao = IdSalaoSelecionado;

            var retorno = await _agendamentoAppService.Incluir(obj);
            if(retorno.Sucesso)
                await AdicionarClaim(CarregarInformacaoTelefoneCliente(obj.TelefoneCliente));

            return Json(retorno);
        }

        [HttpGet]
        [Route("/cancelar-agendamento/{idAgendamento}")]
        public async Task<IActionResult> Cancelar(Guid idAgendamento)
        {
            var retorno = await _agendamentoAppService.Cancelar(idAgendamento);
            return Json(retorno);
        }

        private Claim CarregarInformacaoTelefoneCliente(string telefone)
        {
            return new Claim(ClaimTypes.MobilePhone, telefone);
        }
    }
}