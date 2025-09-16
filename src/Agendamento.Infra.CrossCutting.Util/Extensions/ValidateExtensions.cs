using Agendamento.Infra.CrossCutting.Util.Enums;
using System.Text;
using System.Text.RegularExpressions;

namespace Agendamento.Infra.CrossCutting.Util.Extensions
{
    public static class ValidateExtensions
    {
        public static string GerarNumeroRandom()
        {
            string chars = "0123456789";

            string numeroAleatorio = "";
            Random random = new Random();
            for (int f = 0; f < 6; f++)
            {
                numeroAleatorio = numeroAleatorio + chars.Substring(random.Next(0, chars.Length - 1), 1);
            }

            return numeroAleatorio;
        }

        public static string GerarSenhaRandom()
        {
            string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVXYZabcdefghijklmnopqrstuvxyz";

            string senhaAleatoria = "";
            Random random = new Random();
            for (int f = 0; f < 12; f++)
            {
                senhaAleatoria = senhaAleatoria + chars.Substring(random.Next(0, chars.Length - 1), 1);
            }

            return senhaAleatoria;
        }

        public static bool ValidarFormatoCelular(string celular)
        {
            if (string.IsNullOrWhiteSpace(celular))
                return false;

            Regex phoneRegex = new Regex(@"^\(\d{2}\)\s\d{4,5}-\d{4}$", RegexOptions.Compiled);
            return phoneRegex.IsMatch(celular);
        }

        public static bool ValidarFormatoCEP(string cep)
        {
            if (string.IsNullOrWhiteSpace(cep))
                return false;

            Regex phoneRegex = new Regex(@"^\d{5}-\d{3}$", RegexOptions.Compiled);
            return phoneRegex.IsMatch(cep);
        }

        public static bool ValidarFormatoCPF(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return false;

            Regex cpfRegex = new Regex(@"^\d{3}\.?\d{3}\.?\d{3}-?\d{2}$", RegexOptions.Compiled);
            return cpfRegex.IsMatch(cpf);
        }

        public static bool ValidarFormatoCNPJ(string cnpj)
        {
            if (string.IsNullOrWhiteSpace(cnpj))
                return false;

            Regex cnpjRegex = new Regex(@"^\d{2}\.?\d{3}\.?\d{3}/?\d{4}-?\d{2}$", RegexOptions.Compiled);
            return cnpjRegex.IsMatch(cnpj);
        }

        public static bool ValidarSeExisteSobrenome(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            var parts = name.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return parts.Length > 1 && parts[1].Length > 1;
        }

        public static bool ValidarApenasLetras(string letras)
        {
            if (string.IsNullOrWhiteSpace(letras))
                return false;

            Regex phoneRegex = new Regex(@"^[a-zA-Z]+$", RegexOptions.Compiled);
            return phoneRegex.IsMatch(letras);
        }

        public static string ApenasNumeros(string numeros)
        {
            if (string.IsNullOrWhiteSpace(numeros))
                return string.Empty;

            return Regex.Replace(numeros, @"\D", string.Empty);
        }

        public static string ApenasLetrasENumeros(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            return Regex.Replace(input, @"[^a-zA-Z0-9]", string.Empty);
        }

        public static bool ValidarURL(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            Regex apenasLetrasENumeros = new Regex(@"^[a-zA-Z0-9\-._~]+$", RegexOptions.Compiled);
            return apenasLetrasENumeros.IsMatch(input);
        }

        public static string RemoverAcentos(string input)
        {
            string normalized = input.Normalize(NormalizationForm.FormD);
            string withoutAccents = Regex.Replace(normalized, @"\p{Mn}", "");

            return Regex.Replace(withoutAccents, @"[^a-zA-Z0-9\s]", string.Empty);
        }

        public static string VerificarValidadePlano(this DateTime validadePlano, decimal valorAtualPlano)
        {
            var dataAtual = DateTime.Today;
            var diasRestantes = (validadePlano.Date - dataAtual.Date).TotalDays;

            if (validadePlano == DateTime.MinValue)
                return ValidadePlanoEnum.PrimeiroPagamento.PegarDescricao();

            if (valorAtualPlano is 0 && diasRestantes >= 0)
                return ValidadePlanoEnum.PagamentoPlanoTeste.PegarDescricao();

            if (diasRestantes < 0)
                return ValidadePlanoEnum.PagamentoAtrasado.PegarDescricao();

            if (diasRestantes <= 7)
                return ValidadePlanoEnum.PagamentoPendente.PegarDescricao();

            return ValidadePlanoEnum.PagamentoEmDia.PegarDescricao();
        }

        public static string TransformarEmURL(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            return RemoverAcentos(input.TrimStart().TrimEnd()).ToLower().Replace(' ', '-');
        }
    }
}