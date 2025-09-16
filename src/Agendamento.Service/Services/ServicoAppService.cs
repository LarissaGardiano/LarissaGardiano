using Agendamento.Infra.CrossCutting.Util;
using Agendamento.Service.Context;
using Agendamento.Service.Entidades.Validation;
using Agendamento.Service.Entidades;
using Agendamento.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Agendamento.Infra.CrossCutting.Util.Extensions;
using Dapper;
using System.Globalization;
using Agendamento.Infra.CrossCutting.Util.Enums;
using Microsoft.Data.SqlClient;

namespace Agendamento.Service.Services
{
    public class ServicoAppService : IServicoAppService
    {
        private readonly AgendamentoContext _context;
        private readonly IAgendamentoAppService _agendamentoAppService;

        public ServicoAppService(
            AgendamentoContext context,
            IAgendamentoAppService agendamentoAppService)
        {
            _context = context;
            _agendamentoAppService = agendamentoAppService;
        }

        public async Task<Servico> Consultar(Guid id)
        {
            var query = @"
                SELECT 
                    ID_SERVICO AS IdServico,
                    ID_SALAO AS IdSalao,
                    NM_SERVICO AS Nome,
                    VL_SERVICO AS Valor,
                    NM_OBSERVACAO AS Observacao,
                    NR_DURACAO AS Duracao,
                    DV_STATUS AS Status,
                    ID_ATENDENTE_INCLUSAO AS IdAtendenteInclusao,
                    DT_INCLUSAO AS DataInclusao,
                    ID_ATENDENTE_ALTERACAO AS IdAtendenteAlteracao,
                    DT_ALTERACAO AS DataAlteracao
                FROM TB_SERVICO
                WHERE ID_SERVICO = @id";

            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                return await connection.QuerySingleOrDefaultAsync<Servico>(query, new { id });
            }
        }

        public async Task<IEnumerable<Servico>> Listar(Guid id)
        {
            var query = @"
                SELECT 
                    ID_SERVICO AS IdServico,
                    ID_SALAO AS IdSalao,
                    NM_SERVICO AS Nome,
                    VL_SERVICO AS Valor,
                    NM_OBSERVACAO AS Observacao,
                    NR_DURACAO AS Duracao,
                    DV_STATUS AS Status,
                    ID_ATENDENTE_INCLUSAO AS IdAtendenteInclusao,
                    DT_INCLUSAO AS DataInclusao,
                    ID_ATENDENTE_ALTERACAO AS IdAtendenteAlteracao,
                    DT_ALTERACAO AS DataAlteracao
                FROM TB_SERVICO
                WHERE ID_SALAO = @id";

            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                return await connection.QueryAsync<Servico>(query, new { id });
            }
        }

        public async Task<IEnumerable<Servico>> ListarAtivos(Guid id)
        {
            var query = @"
                SELECT 
                    ID_SERVICO AS IdServico,
                    ID_SALAO AS IdSalao,
                    NM_SERVICO AS Nome,
                    VL_SERVICO AS Valor,
                    NM_OBSERVACAO AS Observacao,
                    NR_DURACAO AS Duracao,
                    DV_STATUS AS Status,
                    ID_ATENDENTE_INCLUSAO AS IdAtendenteInclusao,
                    DT_INCLUSAO AS DataInclusao,
                    ID_ATENDENTE_ALTERACAO AS IdAtendenteAlteracao,
                    DT_ALTERACAO AS DataAlteracao
                FROM TB_SERVICO
                WHERE ID_SALAO = @id
                AND DV_STATUS = 1";

            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                return await connection.QueryAsync<Servico>(query, new { id });
            }
        }

        public async Task<RetornoGenerico> Incluir(Servico obj)
        {
            var validacao = ValidarCampos(obj);

            if (!validacao.Sucesso)
                return validacao;

            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                string query = MontandoQuery(obj, true);
                var retornoProc = new DynamicParameters().OutputParameter();
                await connection.ExecuteAsync(query, retornoProc);

                bool sucesso = retornoProc.Get<int>("@cd_retorno") is 0;
                return new RetornoGenerico(sucesso, sucesso ? "Serviço incluído com sucesso" : "Não foi possível realizar a requisição, contate o administrador do sistema");
            }
        }

        public async Task<RetornoGenerico> Alterar(Servico obj)
        {
            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                string query = MontandoQuery(obj, false);
                var retornoProc = new DynamicParameters().OutputParameter();
                await connection.ExecuteAsync(query, retornoProc);

                bool sucesso = retornoProc.Get<int>("@cd_retorno") is 0;
                return new RetornoGenerico(sucesso, sucesso ? "Serviço alterado com sucesso" : "Não foi possível realizar a requisição, contate o administrador do sistema");
            }
        }

        private static string MontandoQuery(Servico obj, bool ehIncluir)
        {
            return SqlParameterExtensions.PrepararProcedure("p_incluir_alterar_tb_servico", new Dictionary<string, object>
            {
                { "id_servico", ehIncluir ? null : obj.IdServico },
                { "id_salao", obj.IdSalao },
                { "nm_servico", obj.Nome },
                { "vl_servico", obj.Valor.ToString(CultureInfo.InvariantCulture) },
                { "nm_observacao", obj.Observacao },
                { "nr_duracao", obj.Duracao },
                { "dv_status", obj.Status },
                { "id_atendente_inclusao", obj.IdAtendenteInclusao },
                { "id_atendente_alteracao", ehIncluir ? null : obj.IdAtendenteAlteracao },
            });
        }

        private RetornoGenerico ValidarCampos(Servico obj)
        {
            ServicoValidation validation = new();
            var retorno = validation.Validate(obj ?? new Servico());

            if (!retorno.IsValid)
                return new RetornoGenerico(false, retorno.Errors.Select(x => x.ErrorMessage).Take(5).ToList());

            return new RetornoGenerico(true, String.Empty);
        }
    }
}