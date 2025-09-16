
namespace Agendamento.Infra.CrossCutting.Util.ViewModel
{
    public class LoginViewModel
    {
        public string Email { get; set; }
        public string Senha { get; set; }

        public bool IsValid()
        {
            if (String.IsNullOrEmpty(Email) || String.IsNullOrEmpty(Senha))
                return false;

            return true;
        }
    }
}