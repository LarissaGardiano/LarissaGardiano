using Agendamento.Infra.CrossCutting.Util.Extensions;

namespace Agendamento.Infra.CrossCutting.Util.ViewModel
{
    public class DadosAtendenteLogadoViewModel
    {
        public Guid IdAtendente { get; set; }
        public Guid IdSalao { get; set; }

        public string NomeAtendente { get; set; }
        public string CelularAtendente { get; set; }
        public string EmailAtendente { get; set; }
        public bool EhAdministrador { get; set; }
        public string NomeSalao { get; set; }
        public string Url { get; set; }
        public decimal ValorAtualPlano { get; set; }
        public DateTime ValidadePlano { get; set; }
        public string ValidadePlanoExibir => ValidateExtensions.VerificarValidadePlano(ValidadePlano, ValorAtualPlano);
    }
}