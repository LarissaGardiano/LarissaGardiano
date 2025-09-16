using Agendamento.Infra.CrossCutting.Util;
using Agendamento.Service.Context;
using Agendamento.Service.Entidades.Validation;
using Agendamento.Infra.CrossCutting.Util.Extensions;
using Microsoft.EntityFrameworkCore;
using Agendamento.Service.Interfaces;
using Agendamento.Infra.CrossCutting.Util.ViewModel;
using Agendamento.Infra.CrossCutting.Util.Enums;
using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Agendamento.Service.Services
{
    public class AgendamentoAppService : IAgendamentoAppService
    {
        private readonly IHorarioAgendaAppService _horarioAgendaAppService;
        private readonly AgendamentoContext _context;

        public AgendamentoAppService(
            IHorarioAgendaAppService horarioAgendaAppService,
            AgendamentoContext context)
        {
            _horarioAgendaAppService = horarioAgendaAppService;
            _context = context;
        }

        public async Task<IEnumerable<MeuAgendamentoViewModel>> MeusAgendamentosPendentes(string telefone, Guid idSalao)
        {
            var statusAguardando = StatusAgendamentoEnum.A.ToString();
            var query = @"
                SELECT
                    ATENDENTE.NM_IMAGEM AS ImagemAtendente,
                    AGENDAMENTO.ID_AGENDAMENTO AS IdAgendamento,
                    ATENDENTE.NM_ATENDENTE AS NomeAtendente,
                    SERVICO.NM_SERVICO AS NomeServico,
                    AGENDAMENTO.DT_AGENDAMENTO AS DataAgendamento,
                    AGENDAMENTO.VL_SERVICO AS ValorServico,
                    AGENDAMENTO.NM_STATUS AS StatusAgendamento,
                    AGENDAMENTO.DT_INCLUSAO AS DataCriacao
                FROM TB_AGENDAMENTO agendamento
                INNER JOIN TB_ATENDENTE atendente ON agendamento.ID_ATENDENTE = atendente.ID_ATENDENTE
                INNER JOIN TB_SERVICO servico ON agendamento.ID_SERVICO = servico.ID_SERVICO
                WHERE agendamento.NM_TELEFONE_CLIENTE = @telefone
                AND agendamento.ID_SALAO = @idSalao
                AND agendamento.NM_STATUS = @statusAguardando
                ORDER BY agendamento.DT_INCLUSAO DESC";

            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                return await connection.QueryAsync<MeuAgendamentoViewModel>(query, new { telefone, idSalao, statusAguardando });
            }
        }

        public async Task<IEnumerable<MeuAgendamentoViewModel>> MeusAgendamentosHistorico(string telefone, Guid idSalao)
        {
            var statusCancelado = StatusAgendamentoEnum.C.ToString();
            var statusFinalizado = StatusAgendamentoEnum.F.ToString();

            var query = @"
                SELECT TOP 20
                    ATENDENTE.NM_IMAGEM AS ImagemAtendente,
                    AGENDAMENTO.ID_AGENDAMENTO AS IdAgendamento,
                    ATENDENTE.NM_ATENDENTE AS NomeAtendente,
                    SERVICO.NM_SERVICO AS NomeServico,
                    AGENDAMENTO.DT_AGENDAMENTO AS DataAgendamento,
                    AGENDAMENTO.VL_SERVICO AS ValorServico,
                    AGENDAMENTO.NM_STATUS AS StatusAgendamento,
                    AGENDAMENTO.DT_INCLUSAO AS DataCriacao
                FROM TB_AGENDAMENTO agendamento
                INNER JOIN TB_ATENDENTE atendente ON agendamento.ID_ATENDENTE = atendente.ID_ATENDENTE
                INNER JOIN TB_SERVICO servico ON agendamento.ID_SERVICO = servico.ID_SERVICO
                WHERE agendamento.NM_TELEFONE_CLIENTE = @telefone
                AND agendamento.ID_SALAO = @idSalao
                AND agendamento.NM_STATUS IN (@statusCancelado, @statusFinalizado)
                ORDER BY agendamento.DT_INCLUSAO DESC";

            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                return await connection.QueryAsync<MeuAgendamentoViewModel>(query, new { telefone, idSalao, statusCancelado, statusFinalizado });
            }
        }

        public async Task<IEnumerable<OutroAgendamentoViewModel>> OutrosAgendamentos(Guid idAtendente, Guid idSalao, DateTime data)
        {
            var query = @$"
                SELECT
                    ATENDENTE.NM_ATENDENTE AS NomeAtendente,
                    SERVICO.NM_SERVICO AS NomeServico,
                    AGENDAMENTO.NM_CLIENTE AS NomeCliente,
                    AGENDAMENTO.DT_AGENDAMENTO AS DataAgendamento,
                    AGENDAMENTO.NM_STATUS AS StatusAgendamento
                FROM TB_AGENDAMENTO agendamento
                INNER JOIN TB_ATENDENTE atendente ON agendamento.ID_ATENDENTE = atendente.ID_ATENDENTE
                INNER JOIN TB_SERVICO servico ON agendamento.ID_SERVICO = servico.ID_SERVICO
                WHERE atendente.ID_ATENDENTE = @idAtendente
                AND agendamento.ID_SALAO = @idSalao
                AND CAST(agendamento.DT_AGENDAMENTO AS DATE) = CAST(@data AS DATE)
                AND agendamento.NM_STATUS IN ('{StatusAgendamentoEnum.A.ToString()}', '{StatusAgendamentoEnum.F.ToString()}')
                ORDER BY agendamento.DT_AGENDAMENTO DESC";

            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                return await connection.QueryAsync<OutroAgendamentoViewModel>(query, new { idAtendente, idSalao, data });
            }
        }

        public async Task<RetornoGenerico> Cancelar(Guid idAgendamento)
        {
            string query = MontandoQuery(new Entidades.Agendamento
            {
                IdAgendamento = idAgendamento,
                Status = StatusAgendamentoEnum.C.ToString()
            }, false);

            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                var retornoProc = new DynamicParameters().OutputParameter();
                await connection.ExecuteAsync(query, retornoProc);

                bool sucesso = retornoProc.Get<int>("@cd_retorno") is 0;
                return new RetornoGenerico(sucesso, sucesso ? "Agendamento cancelado com sucesso" : "Não foi possível realizar a requisição, contate o administrador do sistema");
            }
        }

        public async Task<RetornoGenerico> Finalizar(Guid idAgendamento)
        {
            string query = MontandoQuery(new Entidades.Agendamento
            {
                IdAgendamento = idAgendamento,
                Status = StatusAgendamentoEnum.F.ToString()
            }, false);

            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                var retornoProc = new DynamicParameters().OutputParameter();
                await connection.ExecuteAsync(query, retornoProc);

                bool sucesso = retornoProc.Get<int>("@cd_retorno") is 0;
                return new RetornoGenerico(sucesso, sucesso ? "Agendamento finalizado com sucesso" : "Não foi possível realizar a requisição, contate o administrador do sistema");
            }
        }

        public async Task<RetornoGenerico> Incluir(Entidades.Agendamento obj)
        {
            var validacao = await ValidarCampos(obj);

            if (!validacao.Sucesso)
                return validacao;

            obj.Status = StatusAgendamentoEnum.A.ToString();

            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                var retornoProc = new DynamicParameters().OutputParameterWithId();
                await connection.ExecuteAsync(MontandoQuery(obj, true), retornoProc);

                bool sucesso = retornoProc.Get<int>("@cd_retorno") is 0;
                if (sucesso)
                {
                    Guid idAgendamento = retornoProc.Get<Guid>("@id_retorno");
                }

                return new RetornoGenerico(sucesso, sucesso ? "Agendamento incluído com sucesso" : "Não foi possível realizar a requisição, contate o administrador do sistema");
            }
        }

        public async Task<IEnumerable<CalendarioAgendaViewModel>> CalendarioAgendaDoDia(Guid? idAtendente, DateTime? data, Guid? idAgendamento = null)
        {
            var query = SqlParameterExtensions.PrepararProcedure("p_consulta_agenda_atendente_dia", new Dictionary<string, object>
            {
                { "id_agendamento", idAgendamento },
                { "id_atendente", idAtendente },
                { "dt_agendamento", data }
            }, false);

            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                return await connection.QueryAsync<CalendarioAgendaViewModel>(query);
            }
        }

        public async Task<IEnumerable<Entidades.Agendamento>> ListarAgendamentosPorPeriodoEStatus(Guid idAtendente, DateTime dataInicio, DateTime dataTermino, StatusAgendamentoEnum status)
        {
            string statusDescricao = status.ToString();
            var query = @$"
                SELECT 
                    ID_AGENDAMENTO AS IdAgendamento,
                    NM_TELEFONE_CLIENTE AS TelefoneCliente,
                    NM_CLIENTE AS NomeCliente,
                    ID_SALAO AS IdSalao,
                    ID_SERVICO AS IdServico,
                    DT_AGENDAMENTO AS DataAgendamento,
                    NM_STATUS AS Status,
                    VL_SERVICO AS Valor,
                    DT_INCLUSAO AS DataInclusao,
                    ID_ATENDENTE AS IdAtendente
                FROM TB_AGENDAMENTO
                WHERE ID_ATENDENTE = @idAtendente
                AND DT_AGENDAMENTO >= @dataInicio
                AND DT_AGENDAMENTO <= @dataTermino
                AND NM_STATUS = @statusDescricao";

            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                return await connection.QueryAsync<Entidades.Agendamento>(query, new { idAtendente, dataInicio, dataTermino, statusDescricao });
            }
        }

        private static string MontandoQuery(Entidades.Agendamento obj, bool incluirIdRetorno = false)
        {
            return SqlParameterExtensions.PrepararProcedure("p_incluir_alterar_tb_agendamento", new Dictionary<string, object>
            {
                { "id_agendamento", obj.IdAgendamento },
                { "id_salao", obj.IdSalao },
                { "id_atendente", obj.IdAtendente },
                { "id_servico", obj.IdServico },
                { "nm_telefone_cliente", obj.TelefoneCliente },
                { "nm_cliente", obj.NomeCliente },
                { "dt_agendamento", obj.DataAgendamento },
                { "nm_status", obj.Status }
            }, true, incluirIdRetorno);
        }

        private async Task<RetornoGenerico> ValidarCampos(Entidades.Agendamento obj)
        {
            var estaDisponivel = await _horarioAgendaAppService.VerificarDisponibilidade(obj.IdAtendente, obj.DataAgendamento);

            AgendamentoValidation validation = new(estaDisponivel);
            var retorno = validation.Validate(obj ?? new Entidades.Agendamento());

            if (!retorno.IsValid)
                return new RetornoGenerico(false, retorno.Errors.Select(x => x.ErrorMessage).Take(5).ToList());

            return new RetornoGenerico(true, String.Empty);
        }
    }
}