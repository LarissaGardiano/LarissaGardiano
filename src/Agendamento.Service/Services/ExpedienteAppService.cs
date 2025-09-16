using Agendamento.Infra.CrossCutting.Util;
using Agendamento.Infra.CrossCutting.Util.Extensions;
using Agendamento.Service.Context;
using Agendamento.Service.Entidades;
using Agendamento.Service.Entidades.Validation;
using Agendamento.Service.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Agendamento.Service.Services
{
    public class ExpedienteAppService : IExpedienteAppService
    {
        private readonly AgendamentoContext _context;
        public ExpedienteAppService(
            AgendamentoContext context)
        {
            _context = context;
        }

        public async Task<Expediente> ConsultarPorAtendente(Guid id)
        {
            var query = @"
                SELECT 
                        ID_EXPEDIENTE AS IdExpediente,
                        ID_ATENDENTE AS IdAtendente,
                        DV_DOMINGO AS Domingo,
                        HR_INICIO_DOMINGO AS HoraInicioDomingo,
                        HR_FIM_DOMINGO AS HoraFimDomingo,
                        DV_SEGUNDA AS Segunda,
                        HR_INICIO_SEGUNDA AS HoraInicioSegunda,
                        HR_FIM_SEGUNDA AS HoraFimSegunda,
                        DV_TERCA AS Terca,
                        HR_INICIO_TERCA AS HoraInicioTerca,
                        HR_FIM_TERCA AS HoraFimTerca,
                        DV_QUARTA AS Quarta,
                        HR_INICIO_QUARTA AS HoraInicioQuarta,
                        HR_FIM_QUARTA AS HoraFimQuarta,
                        DV_QUINTA AS Quinta,
                        HR_INICIO_QUINTA AS HoraInicioQuinta,
                        HR_FIM_QUINTA AS HoraFimQuinta,
                        DV_SEXTA AS Sexta,
                        HR_INICIO_SEXTA AS HoraInicioSexta,
                        HR_FIM_SEXTA AS HoraFimSexta,
                        DV_SABADO AS Sabado,
                        HR_INICIO_SABADO AS HoraInicioSabado,
                        HR_FIM_SABADO AS HoraFimSabado,
                        ID_ATENDENTE_INCLUSAO AS IdAtendenteInclusao,
                        DT_INCLUSAO AS DataInclusao,
                        ID_ATENDENTE_ALTERACAO AS IdAtendenteAlteracao,
                        DT_ALTERACAO AS DataAlteracao
                    FROM TB_EXPEDIENTE
                    WHERE ID_ATENDENTE = @id";

            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                return await connection.QuerySingleOrDefaultAsync<Expediente>(query, new { id });
            }
        }

        public async Task<RetornoGenerico> Alterar(Expediente obj)
        {
            var validacao = ValidarCampos(obj);

            if (!validacao.Sucesso) 
                return validacao;

            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                string query = MontandoQuery(obj, false);
                var retornoProc = new DynamicParameters().OutputParameter();
                await connection.ExecuteAsync(query, retornoProc);

                bool sucesso = retornoProc.Get<int>("@cd_retorno") is 0;
                return new RetornoGenerico(sucesso, sucesso ? "Expediente alterado com sucesso" : "Não foi possível realizar a requisição, contate o administrador do sistema");
            }
        }

        private static string MontandoQuery(Expediente obj, bool ehIncluir)
        {
            return SqlParameterExtensions.PrepararProcedure("p_incluir_alterar_tb_expediente", new Dictionary<string, object>
            {
                { "id_expediente", ehIncluir ? null : obj.IdExpediente },
                { "id_atendente", obj.IdAtendente },
                { "dv_domingo", obj.Domingo },
                { "hr_inicio_domingo", obj.HoraInicioDomingo },
                { "hr_fim_domingo", obj.HoraFimDomingo },
                { "dv_segunda", obj.Segunda },
                { "hr_inicio_segunda", obj.HoraInicioSegunda },
                { "hr_fim_segunda", obj.HoraFimSegunda },
                { "dv_terca", obj.Terca },
                { "hr_inicio_terca", obj.HoraInicioTerca },
                { "hr_fim_terca", obj.HoraFimTerca },
                { "dv_quarta", obj.Quarta },
                { "hr_inicio_quarta", obj.HoraInicioQuarta },
                { "hr_fim_quarta", obj.HoraFimQuarta },
                { "dv_quinta", obj.Quinta },
                { "hr_inicio_quinta", obj.HoraInicioQuinta },
                { "hr_fim_quinta", obj.HoraFimQuinta },
                { "dv_sexta", obj.Sexta },
                { "hr_inicio_sexta", obj.HoraInicioSexta },
                { "hr_fim_sexta", obj.HoraFimSexta },
                { "dv_sabado", obj.Sabado },
                { "hr_inicio_sabado", obj.HoraInicioSabado },
                { "hr_fim_sabado", obj.HoraFimSabado },
                { "id_atendente_inclusao", ehIncluir ? obj.IdAtendenteInclusao : null },
                { "id_atendente_alteracao", ehIncluir ? null : obj.IdAtendenteAlteracao }
            });
        }

        private RetornoGenerico ValidarCampos(Expediente obj)
        {
            ExpedienteValidation validation = new();
            var retorno = validation.Validate(obj ?? new Expediente());

            if (!retorno.IsValid)
                return new RetornoGenerico(false, retorno.Errors.Select(x => x.ErrorMessage).Take(5).ToList());

            return new RetornoGenerico(true, String.Empty);
        }
    }
}