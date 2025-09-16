
namespace Agendamento.Service.Entidades
{
    public class Salao
    {
        public Guid IdSalao { get; set; }
        public Guid IdPlano { get; set; }
        public Guid? IdAtendenteAlteracao { get; set; }

        public string Nome { get; set; }
        public string Url { get; set; }
        public string CnpjOuCpf { get; set; }
        public TimeSpan InicioExpediente { get; set; }
        public TimeSpan TerminoExpediente { get; set; }
        public int IntervaloMinutos { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public bool Status { get; set; }
    }
}