using Dapper;
using System.Data;
using System.Text;

namespace Agendamento.Infra.CrossCutting.Util.Extensions
{
    public static class SqlParameterExtensions
    {
        const string Schema = "dbo";

        public static DynamicParameters OutputParameter(this DynamicParameters parameters)
        {
            parameters.Add("@cd_retorno", dbType: DbType.Int32, direction: ParameterDirection.Output);
            return parameters;
        }

        public static DynamicParameters OutputParameterWithId(this DynamicParameters parameters)
        {
            parameters.Add("@cd_retorno", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@id_retorno", dbType: DbType.Guid, direction: ParameterDirection.Output);
            return parameters;
        }

        public static string PrepararProcedure(string nome, Dictionary<string, object> entradas, bool incluirRetorno = true, bool incluirIdRetorno = false)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"EXEC {Schema}.{nome} ");

            bool ehPrimeiro = true;
            foreach (var item in entradas)
            {
                if (!ehPrimeiro)
                    sb.Append(", ");

                sb.Append($"@{item.Key} = ");

                if (item.Value is int)
                    sb.Append(item.Value);

                else if (item.Value is DateTime)
                    sb.Append((DateTime)item.Value == DateTime.MinValue ? "NULL" : $"'{((DateTime)item.Value).ToString("dd-MM-yyyy HH:mm:ss")}'");

                else if (item.Value is DateOnly)
                    sb.Append($"'{((DateOnly)item.Value).ToString("dd-MM-yyyy")}'");

                else if (item.Value is bool)
                    sb.Append(((bool)item.Value) ? "'1'" : "'0'");

                else if (item.Value is string)
                    sb.Append((String.IsNullOrEmpty((string)item.Value)) ? "NULL" : $"'{(string)item.Value}'");

                else if (item.Value is Guid)
                    sb.Append((Guid)item.Value == Guid.Empty ? "NULL" : $"'{(Guid)item.Value}'");

                else if (item.Value is null)
                    sb.Append("NULL");

                else
                    sb.Append($"'{item.Value}'");

                ehPrimeiro = false;
            }

            if (incluirRetorno)
                sb.Append(", @cd_retorno = @cd_retorno OUTPUT");

            if (incluirIdRetorno)
                sb.Append(", @id_retorno = @id_retorno OUTPUT");

            return sb.ToString();
        }

        public static string PrepararProcedure(string nome)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"EXEC {Schema}.{nome}");
            return sb.ToString();
        }
    }
}