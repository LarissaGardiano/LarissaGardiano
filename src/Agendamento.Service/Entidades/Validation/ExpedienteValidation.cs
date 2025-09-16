using FluentValidation;

namespace Agendamento.Service.Entidades.Validation
{
    public class ExpedienteValidation : AbstractValidator<Expediente>
    {
        public ExpedienteValidation()
        {
            RuleFor(x => x.IdAtendente).NotEmpty()
                .WithMessage("Informe o atendente");

            RuleFor(x => x)
                .Must(x => !x.Segunda || x.HoraInicioSegunda < x.HoraFimSegunda)
                .WithMessage("Início segunda não pode ser maior ou igual ao término");

            RuleFor(x => x)
               .Must(x => !x.Terca || x.HoraInicioTerca < x.HoraFimTerca)
               .WithMessage("Início terça não pode ser maior ou igual ao término");

            RuleFor(x => x)
               .Must(x => !x.Quarta || x.HoraInicioQuarta < x.HoraFimQuarta)
               .WithMessage("Início quarta não pode ser maior ou igual ao término");

            RuleFor(x => x)
               .Must(x => !x.Quinta || x.HoraInicioQuinta < x.HoraFimQuinta)
               .WithMessage("Início quinta não pode ser maior ou igual ao término");

            RuleFor(x => x)
               .Must(x => !x.Sexta || x.HoraInicioSexta < x.HoraFimSexta)
               .WithMessage("Início sexta não pode ser maior ou igual ao término");

            RuleFor(x => x)
               .Must(x => !x.Sabado || x.HoraInicioSabado < x.HoraFimSabado)
               .WithMessage("Início sábado não pode ser maior ou igual ao término");

            RuleFor(x => x)
               .Must(x => !x.Domingo || x.HoraInicioDomingo < x.HoraFimDomingo)
               .WithMessage("Início domingo não pode ser maior ou igual ao término");
        }
    }
}