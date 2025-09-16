using Agendamento.Infra.CrossCutting.Util.Enums;

namespace Agendamento.Service.Entidades
{
    public class Agendamento
    {
        public Guid IdAgendamento { get; set; }
        public Guid IdSalao { get; set; }
        public Guid IdAtendente { get; set; }
        public Guid IdServico { get; set; }
        public decimal Valor { get; set; }
        public string TelefoneCliente { get; set; }
        public string NomeCliente { get; set; }
        public string Status { get; set; }
        public StatusAgendamentoEnum StatusEnum
        {
            get
            {
                if (String.IsNullOrEmpty(Status))
                    return StatusAgendamentoEnum.A;

                Enum.TryParse(Status, out StatusAgendamentoEnum statusEnum);
                return statusEnum;
            }
        }

        public DateTime DataAgendamento { get; set; }
        public DateTime DataInclusao { get; set; }
    }
}