
using Agendamento.Infra.CrossCutting.Util.Enums;

namespace Agendamento.Infra.CrossCutting.Util.ViewModel
{
    public class MeuAgendamentoViewModel
    {
        public Guid IdAgendamento { get; set; }
        public string ImagemAtendente { get; set; }
        public string NomeAtendente { get; set; }
        public string NomeServico { get; set; }
        public DateTime DataAgendamento { get; set; }
        public decimal ValorServico { get; set; }
        public string StatusAgendamento { get; set; }
        public DateTime DataCriacao { get; set; }

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