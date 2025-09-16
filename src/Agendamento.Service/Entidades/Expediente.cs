
namespace Agendamento.Service.Entidades
{
    public class Expediente
    {
        public Guid IdExpediente { get; set; }
        public Guid IdAtendente { get; set; }

        public bool Domingo { get; set; }
        public TimeSpan? HoraInicioDomingo { get; set; }
        public TimeSpan? HoraFimDomingo { get; set; }

        public bool Segunda { get; set; }
        public TimeSpan? HoraInicioSegunda { get; set; }
        public TimeSpan? HoraFimSegunda { get; set; }

        public bool Terca { get; set; }
        public TimeSpan? HoraInicioTerca { get; set; }
        public TimeSpan? HoraFimTerca { get; set; }

        public bool Quarta { get; set; }
        public TimeSpan? HoraInicioQuarta { get; set; }
        public TimeSpan? HoraFimQuarta { get; set; }

        public bool Quinta { get; set; }
        public TimeSpan? HoraInicioQuinta { get; set; }
        public TimeSpan? HoraFimQuinta { get; set; }

        public bool Sexta { get; set; }
        public TimeSpan? HoraInicioSexta { get; set; }
        public TimeSpan? HoraFimSexta { get; set; }

        public bool Sabado { get; set; }
        public TimeSpan? HoraInicioSabado { get; set; }
        public TimeSpan? HoraFimSabado { get; set; }

        public DateTime DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public Guid IdAtendenteInclusao { get; set; }
        public Guid? IdAtendenteAlteracao { get; set; }
    }
}