using Agendamento.Infra.CrossCutting.Util.Extensions;
using FluentValidation;

namespace Agendamento.Service.Entidades.Validation
{
    public class AgendamentoValidation : AbstractValidator<Agendamento>
    {
        public AgendamentoValidation(bool estaDisponivel)
        {
            RuleFor(x => x.NomeCliente)
              .NotEmpty()
              .WithMessage("Informe o nome")
              .DependentRules(() =>
              {
                  RuleFor(x => x.NomeCliente)
                      .Must(ValidateExtensions.ValidarSeExisteSobrenome)
                      .WithMessage("Informe o sobrenome");
              });

            RuleFor(x => x.TelefoneCliente)
                .NotEmpty()
                .WithMessage("Informe o celular")
                .DependentRules(() =>
                {
                    RuleFor(x => x.TelefoneCliente)
                    .Must(ValidateExtensions.ValidarFormatoCelular)
                    .WithMessage("Informe um celular válido");
                });

            RuleFor(x => x)
               .Must(x => estaDisponivel)
               .WithMessage("Horário indisponível no momento");
        }
    }
}