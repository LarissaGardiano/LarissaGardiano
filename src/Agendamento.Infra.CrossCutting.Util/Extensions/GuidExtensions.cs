
namespace Agendamento.Infra.CrossCutting.Util.Extensions
{
    public static class GuidExtensions
    {
        public static string GuidParaBase64(this Guid guid)
        {
            return Convert.ToBase64String(guid.ToByteArray())
                .TrimEnd('=')
                .Replace("/", "_")
                .Replace("+", "-");
        }

        public static Guid Base64ParaGuid(this string guid)
        {
            string base64 = guid.Replace("_", "/").Replace("-", "+") + "==";
            byte[] bytes = Convert.FromBase64String(base64);
            return new Guid(bytes);
        }
    }
}