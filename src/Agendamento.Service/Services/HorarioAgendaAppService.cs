using Agendamento.Infra.CrossCutting.Util.Extensions;
using Agendamento.Service.Context;
using Agendamento.Service.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Agendamento.Service.Services
{
    public class HorarioAgendaAppService : IHorarioAgendaAppService
    {
        private readonly AgendamentoContext _context;
        public HorarioAgendaAppService(
            AgendamentoContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TimeSpan>> ListarHorariosDisponiveis(Guid idAtendente, Guid idServico, DateOnly data)
        {
            string query = MontandoQueryListagem(idAtendente, idServico, data);
            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                return await connection.QueryAsync<TimeSpan>(query);
            }
        }

        public async Task<bool> VerificarDisponibilidade(Guid idAtendente, DateTime data)
        {
            var dia = data.ToString("dd-MM-yyyy");
            var hora = data.ToString("HH:mm");

            var query = @"
                SELECT 
	                DV_DISPONIVEL
                FROM TB_HORARIOS_DISPONIVEIS_AGENDA
                WHERE ID_ATENDENTE = @idAtendente
                AND DT_DATA = @dia
                AND HR_HORARIO = @hora";

            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                return await connection.QuerySingleOrDefaultAsync<bool>(query, new { idAtendente, dia, hora });
            }
        }

        private static string MontandoQueryListagem(Guid idAtendente, Guid idServico, DateOnly data)
        {
            return SqlParameterExtensions.PrepararProcedure("p_consulta_horarios_disponiveis_agenda ", new Dictionary<string, object>
            {
                { "id_atendente", idAtendente},
                { "id_servico", idServico },
                { "dt_agendamento", data }
            }, false);
        }
    }
}