
using Microsoft.AspNetCore.Http;

namespace Agendamento.Service.Entidades
{
    public class Atendente
    {
        public Guid IdAtendente { get; set; }
        public Guid IdSalao { get; set; }
        public Guid? IdAtendenteAlteracao { get; set; }

        public string Nome { get; set; }
        public string Celular { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string ImagemAtendente { get; set; }
        public bool EhAdministrador { get; set; }
        public bool Status { get; set; }
        public TimeSpan InicioAlmoco { get; set; }
        public TimeSpan TerminoAlmoco { get; set; }

        public IFormFile Imagem { get; set; }

        public string TokenTrocaSenha { get; set; }
        public DateTime? PrazoTrocaSenha { get; set; }

        public DateTime DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
    }
}