using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Agendamento.Controllers
{
    public class BaseController : Controller
    {
        public async Task RegistrarClaims(List<Claim> claims)
        {
            ClaimsPrincipal currentUser = HttpContext.User;
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var claimsPrincipal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
        }

        public async Task AdicionarClaim(Claim novaClaim)
        {
            ClaimsPrincipal currentUser = HttpContext.User;
            var claimsContext = new List<Claim>(currentUser.Claims);

            var existingClaim = claimsContext.FirstOrDefault(c => c.Type == novaClaim.Type);
            if (existingClaim is not null)
            {
                claimsContext.Remove(existingClaim);
            }

            claimsContext.Add(novaClaim);

            var identity = new ClaimsIdentity(claimsContext, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(claimsPrincipal);
        }

        protected string TelefoneSelecionado
        {
            get
            {
                var identity = User.Identity as ClaimsIdentity;
                var userData = identity?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.MobilePhone);
                if (string.IsNullOrEmpty(userData?.Value ?? string.Empty))
                    return string.Empty;

                return userData?.Value ?? string.Empty;
            }
        }

        protected Guid IdSalaoSelecionado
        {
            get
            {
                return new Guid("3A0079DD-E919-4B4A-8E62-AB871C769D1A");
            }
        }

        protected string NomeSalaoSelecionado
        {
            get
            {
                return "salao";
            }
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            ViewBag.TelefoneCliente = TelefoneSelecionado;
            ViewBag.NomeSalaoSelecionado = NomeSalaoSelecionado;

            base.OnActionExecuted(context);
        }
    }
}