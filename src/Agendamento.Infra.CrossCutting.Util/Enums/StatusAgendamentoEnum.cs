using System.ComponentModel;

namespace Agendamento.Infra.CrossCutting.Util.Enums
{
    public enum StatusAgendamentoEnum
    {
        [Description("Agendado")]
        A,

        [Description("Cancelado")]
        C,

        [Description("Finalizado")]
        F
    }
}