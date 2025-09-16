using System.ComponentModel;

namespace Agendamento.Infra.CrossCutting.Util.Enums
{
    public enum StatusPagbankEnum
    {
        [Description("AUTHORIZED")]
        AUTHORIZED,

        [Description("PAID")]
        PAID,

        [Description("DECLINED")]
        DECLINED,

        [Description("CANCELED")]
        CANCELED,

        [Description("WAITING")]
        WAITING,

        [Description("DESCONHECIDO")]
        DESCONHECIDO
    }
}