using System.ComponentModel;

namespace Agendamento.Infra.CrossCutting.Util.Enums
{
    public enum ValidadePlanoEnum
    {
        [Description("PrimeiroPagamento")]
        PrimeiroPagamento,

        [Description("PagamentoAtrasado")]
        PagamentoAtrasado,

        [Description("PagamentoPendente")]
        PagamentoPendente,

        [Description("PagamentoEmDia")]
        PagamentoEmDia,

        [Description("PagamentoPlanoTeste")]
        PagamentoPlanoTeste
    }
}