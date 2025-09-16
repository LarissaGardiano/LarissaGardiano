
namespace Agendamento.Service.Entidades
{
    public class Servico
    {
        public Guid IdServico { get; set; }
        public Guid IdSalao { get; set; }
        public string Nome { get; set; }
        public decimal Valor { get; set; }
        public string Observacao { get; set; }
        public int Duracao { get; set; }
        public bool Status { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public Guid IdAtendenteInclusao { get; set; }
        public Guid? IdAtendenteAlteracao { get; set; }
    }
}