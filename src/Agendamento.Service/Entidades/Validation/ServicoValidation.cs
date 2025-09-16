using FluentValidation;

namespace Agendamento.Service.Entidades.Validation
{
    public class ServicoValidation : AbstractValidator<Servico>
    {
        public ServicoValidation()
        {
            RuleFor(x => x.Nome).NotEmpty()
             .WithMessage("Informe o nome");

            RuleFor(x => x.Valor).NotEmpty()
             .WithMessage("Informe o valor");

            RuleFor(x => x.Duracao).NotEmpty()
             .WithMessage("Informe a duração");
        }
    }
}