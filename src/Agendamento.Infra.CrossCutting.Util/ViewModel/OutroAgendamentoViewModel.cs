using Agendamento.Infra.CrossCutting.Util.Enums;

namespace Agendamento.Infra.CrossCutting.Util.ViewModel
{
    public class OutroAgendamentoViewModel
    {
        public string NomeAtendente { get; set; }
        public string NomeServico { get; set; }
        public string NomeCliente { get; set; }
        public DateTime DataAgendamento { get; set; }
        public string StatusAgendamento { get; set; }

        public StatusAgendamentoEnum StatusEnum
        {
            get
            {
                if (String.IsNullOrEmpty(StatusAgendamento))
                    return StatusAgendamentoEnum.A;

                Enum.TryParse(StatusAgendamento, out StatusAgendamentoEnum statusEnum);
                return statusEnum;
            }
        }
    }
}