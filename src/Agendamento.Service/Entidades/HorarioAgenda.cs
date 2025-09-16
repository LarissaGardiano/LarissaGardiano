
namespace Agendamento.Service.Entidades
{
    public class HorarioAgenda
    {
        public Guid IdAtendente { get; set; }
        public DateOnly Data { get; set; }
        public TimeOnly Hora { get; set; }
        public bool Disponivel { get; set; }
    }
}