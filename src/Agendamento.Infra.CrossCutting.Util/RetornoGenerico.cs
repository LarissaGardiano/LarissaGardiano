
namespace Agendamento.Infra.CrossCutting.Util
{
    public class RetornoGenerico
    {
        public RetornoGenerico()
        {
        }

        public RetornoGenerico(bool sucesso)
        {
            Sucesso = sucesso;
            Mensagens = new List<string>();
        }

        public RetornoGenerico(int sucesso, string mensagens)
        {
            Sucesso = sucesso == 1;
            Mensagens = new List<string> { mensagens };
        }

        public RetornoGenerico(bool sucesso, List<string> mensagens)
        {
            Sucesso = sucesso;
            Mensagens = mensagens;
        }

        public RetornoGenerico(bool sucesso, string mensagens)
        {
            Sucesso = sucesso;
            Mensagens = new List<string> { mensagens };
        }

        public RetornoGenerico(bool sucesso, Guid? id, string mensagens)
        {
            Id = id;
            Sucesso = sucesso;
            Mensagens = new List<string> { mensagens };
        }

        public RetornoGenerico(bool sucesso, Guid? id)
        {
            Id = id;
            Sucesso = sucesso;
            Mensagens = new List<string>();
        }

        public RetornoGenerico(bool sucesso, Guid? id, List<string> mensagens)
        {
            Id = id;
            Sucesso = sucesso;
            Mensagens = mensagens;
        }

        public bool Sucesso { get; set; }

        public Guid? Id { get; set; }

        public List<string> Mensagens { get; set; }
    }
}