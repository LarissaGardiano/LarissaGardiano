using Agendamento.Infra.CrossCutting.Util.Extensions;
using FluentValidation;

namespace Agendamento.Service.Entidades.Validation
{
    public class AtendenteValidation : AbstractValidator<Atendente>
    {
        public AtendenteValidation()
        {
            RuleFor(x => x.Nome)
                .NotEmpty()
                .WithMessage("Informe o nome")
                .DependentRules(() =>
                {
                    RuleFor(x => x.Nome)
                        .Must(ValidateExtensions.ValidarSeExisteSobrenome)
                        .WithMessage("Informe o sobrenome");
                });

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Informe o e-mail")
                .EmailAddress()
                .WithMessage("Informe um e-mail válido");

            RuleFor(x => x.Celular)
                 .NotEmpty()
                 .WithMessage("Informe o celular")
                 .DependentRules(() =>
                 {
                     RuleFor(x => x.Celular)
                     .Must(ValidateExtensions.ValidarFormatoCelular)
                     .WithMessage("Informe um celular válido");
                 });

            RuleFor(x => x.InicioAlmoco).NotEmpty()
                .WithMessage("Informe o início almoço");

            RuleFor(x => x.TerminoAlmoco).NotEmpty()
                .WithMessage("Informe o término almoço");

            RuleFor(x => x)
                .Must(x => x.InicioAlmoco < x.TerminoAlmoco)
                .WithMessage("Início almoço não pode ser maior que o término");
        }
    }
}