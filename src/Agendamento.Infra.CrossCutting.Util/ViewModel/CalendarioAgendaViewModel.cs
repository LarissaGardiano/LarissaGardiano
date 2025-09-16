
namespace Agendamento.Infra.CrossCutting.Util.ViewModel
{
    public class CalendarioAgendaViewModel
    {
        public Guid IdAgendamento { get; set; }
        public string NomeCliente { get; set; }
        public string NomeServico { get; set; }
        public string StatusAgendamento { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Termino { get; set; }

        public string InicioExibir { get { return $"{Inicio.Year},{Inicio.Month - 1},{Inicio.Day},{Inicio.Hour},{Inicio.Minute}"; } }
        public string TerminoExibir { get { return $"{Termino.Year},{Termino.Month -1},{Termino.Day},{Termino.Hour},{Termino.Minute}"; } }
    }
}