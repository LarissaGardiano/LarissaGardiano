using Agendamento.Infra.CrossCutting.Util.ViewModel;
using Agendamento.Service.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Agendamento.Controllers
{
    public class AutenticacaoController : Controller
    {
        private readonly IAtendenteAppService _atendenteAppService;
        public AutenticacaoController(
            IAtendenteAppService atendenteAppService)
        {
            _atendenteAppService = atendenteAppService;
        }

        [HttpGet]
        public IActionResult Index(bool alerta = false)
        {
            ViewBag.Alerta = alerta;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Autenticar(LoginViewModel obj)
        {
            if (!obj.IsValid())
            {
                return RedirectToAction("Index", "Autenticacao", new { alerta = true });
            }

            var atendente = await _atendenteAppService.Autenticar(obj);
            if (atendente is null)
            {
                return RedirectToAction("Index", "Autenticacao", new { alerta = true });
            }

            var claims = _atendenteAppService.CriarClaims(atendente);
            await HttpContext.SignInAsync(claims);

            return RedirectToAction("Menu", "Home");
        }
    }
}