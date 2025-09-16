using Agendamento.Infra.CrossCutting.Util.Extensions;
using FluentValidation;

namespace Agendamento.Service.Entidades.Validation
{
    public class SalaoValidation : AbstractValidator<Salao>
    {
        public SalaoValidation(bool existeUrlEmOutroSalao)
        {
            RuleFor(x => x.Nome).NotEmpty()
                .WithMessage("Informe o nome");

            RuleFor(x => x.CnpjOuCpf)
                .NotEmpty()
                .WithMessage("Informe o CPF ou CNPJ")
                .DependentRules(() =>
                {
                    RuleFor(x => x.CnpjOuCpf)
                    .Must(ValidarCPFouCNPJ)
                    .WithMessage("CPF ou CNPJ inválido");
                });

            RuleFor(x => x.Url)
                .NotEmpty()
                .WithMessage("Informe o link")
                .DependentRules(() =>
                {
                    RuleFor(x => x.Url)
                    .Must(ValidateExtensions.ValidarURL)
                    .WithMessage("Link deve conter letras e números e sem espaço")
                    .DependentRules(() =>
                    {
                        if (existeUrlEmOutroSalao)
                        {
                            RuleFor(x => x.Url)
                            .Custom((url, context) =>
                                {
                                    context.AddFailure("Este link já está sendo usado por outro salão");
                                });
                        }
                    });
                });

            RuleFor(x => x.IntervaloMinutos).NotEmpty()
                .WithMessage("Informe o intervalo agendamento");

            RuleFor(x => x.InicioExpediente).NotEmpty()
                .WithMessage("Informe o início expediente");

            RuleFor(x => x.TerminoExpediente).NotEmpty()
                .WithMessage("Informe o término expediente");

            RuleFor(x => x)
                .Must(x => x.InicioExpediente < x.TerminoExpediente)
                .WithMessage("Início do expediente não pode ser maior que o término");
        }

        private bool ValidarCPFouCNPJ(string cpfOuCnpj)
        {
            return ValidateExtensions.ValidarFormatoCPF(cpfOuCnpj) || ValidateExtensions.ValidarFormatoCNPJ(cpfOuCnpj);
        }
    }
}