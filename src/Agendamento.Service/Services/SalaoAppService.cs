using Agendamento.Infra.CrossCutting.Util;
using Agendamento.Infra.CrossCutting.Util.Extensions;
using Agendamento.Service.Context;
using Agendamento.Service.Entidades;
using Agendamento.Service.Entidades.Validation;
using Agendamento.Service.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Agendamento.Service.Services
{
    public class SalaoAppService : ISalaoAppService
    {
        private readonly AgendamentoContext _context;
        public SalaoAppService(
            AgendamentoContext context)
        {
            _context = context;
        }

        public async Task<Salao> Consultar(string nomeSalao)
        {
            var query = @"
                SELECT 
	                ID_SALAO AS idSalao,
                    ID_PLANO AS idPlano,
                    ID_ATENDENTE_ALTERACAO AS idAtendenteAlteracao,
                    NM_COMERCIAL AS nome,
                    NM_URL AS Url,
                    NR_CNPJ_CPF AS cnpjOuCpf,
                    NR_INTERVALO_MINUTOS AS IntervaloMinutos,
                    HR_HORARIO_INICIO AS InicioExpediente,
                    HR_HORARIO_FIM AS TerminoExpediente,
                    DV_STATUS AS status,
                    DT_INCLUSAO AS dataInclusao,
                    DT_ALTERACAO AS dataAlteracao
                FROM TB_SALAO
                WHERE NM_URL = @nomeSalao";

            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                return await connection.QuerySingleOrDefaultAsync<Salao>(query, new { nomeSalao });
            }
        }

        public async Task<bool> ExisteUrlEmOutroSalao(Guid idSalao, string link)
        {
            var query = @"
                SELECT 1
                FROM TB_SALAO
                WHERE ID_SALAO <> @idSalao
                AND NM_URL = @link";

            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                var result = await connection.QueryFirstOrDefaultAsync<int?>(query, new { idSalao, link });
                return result.HasValue;
            }
        }

        public async Task<Salao> Consultar(Guid idSalao)
        {
            var query = @"
                SELECT 
	                ID_SALAO AS idSalao,
                    ID_PLANO AS idPlano,
                    ID_ATENDENTE_ALTERACAO AS idAtendenteAlteracao,
                    NM_COMERCIAL AS nome,
                    NM_URL AS Url,
                    NR_CNPJ_CPF AS cnpjOuCpf,
                    NR_INTERVALO_MINUTOS AS IntervaloMinutos,
                    HR_HORARIO_INICIO AS InicioExpediente,
                    HR_HORARIO_FIM AS TerminoExpediente,
                    DV_STATUS AS status,
                    DT_INCLUSAO AS dataInclusao,
                    DT_ALTERACAO AS dataAlteracao
                FROM TB_SALAO
                WHERE ID_SALAO = @idSalao";

            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                return await connection.QuerySingleOrDefaultAsync<Salao>(query, new { idSalao });
            }
        }

        public async Task<RetornoGenerico> Alterar(Salao obj)
        {
            var validacao = await ValidarCampos(obj);

            if (!validacao.Sucesso)
                return validacao;

            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                await connection.OpenAsync();
                string query = MontandoQuery(obj);
                var retornoProc = new DynamicParameters().OutputParameter();
                await connection.ExecuteAsync(query, retornoProc);

                bool sucesso = retornoProc.Get<int>("@cd_retorno") is 0;
                return new RetornoGenerico(sucesso, sucesso ? "Salão alterado com sucesso" : "Não foi possível realizar a requisição, contate o administrador do sistema");
            }
        }

        private static string MontandoQuery(Salao obj)
        {
            return SqlParameterExtensions.PrepararProcedure("p_incluir_alterar_tb_salao", new Dictionary<string, object>
            {
                { "id_salao", obj.IdSalao },
                { "id_plano", obj.IdPlano },
                { "nm_comercial", obj.Nome },
                { "nm_url", obj.Url },
                { "nr_cnpj_cpf", obj.CnpjOuCpf },
                { "hr_horario_inicio", obj.InicioExpediente },
                { "hr_horario_fim", obj.TerminoExpediente },
                { "nr_intervalo_minutos", obj.IntervaloMinutos },
                { "id_atendente_alteracao", obj.IdAtendenteAlteracao }
            });
        }

        private async Task<RetornoGenerico> ValidarCampos(Salao obj)
        {
            SalaoValidation validation = new(await ExisteUrlEmOutroSalao(obj.IdSalao, obj.Url));
            var retorno = validation.Validate(obj ?? new Salao());

            if (!retorno.IsValid)
                return new RetornoGenerico(false, retorno.Errors.Select(x => x.ErrorMessage).Take(5).ToList());

            return new RetornoGenerico(true, String.Empty);
        }
    }
}