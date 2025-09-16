using Agendamento.Infra.CrossCutting.Util;
using Agendamento.Infra.CrossCutting.Util.Extensions;
using Agendamento.Infra.CrossCutting.Util.Settings;
using Agendamento.Infra.CrossCutting.Util.ViewModel;
using Agendamento.Service.Context;
using Agendamento.Service.Entidades;
using Agendamento.Service.Entidades.Validation;
using Agendamento.Service.Interfaces;
using Dapper;
using Microsoft.AspNet.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Agendamento.Service.Services
{
    public class AtendenteAppService : IAtendenteAppService
    {
        private readonly WebSettings _webSettings;
        private readonly AgendamentoContext _context;

        public AtendenteAppService(
            IOptions<WebSettings> options,
            AgendamentoContext context)
        {
            _webSettings = options.Value;
            _context = context;
        }

        public async Task<Atendente> Consultar(Guid id)
        {
            var query = @"
                SELECT 
                    ID_ATENDENTE AS idAtendente,
                    ID_SALAO AS idSalao, 
                    NM_ATENDENTE AS nome,
                    NM_CELULAR AS celular,
                    NM_EMAIL AS email,
                    NM_SENHA AS senha,
                    DV_ADMINISTRADOR AS ehAdministrador,
                    DV_STATUS AS status,
                    HR_INICIO_ALMOCO AS InicioAlmoco,
                    HR_FIM_ALMOCO AS TerminoAlmoco,
                    NM_IMAGEM as ImagemAtendente,
                    NM_TOKEN_ALTERA_SENHA as tokenTrocaSenha,
                    DT_PRAZO_ALTERA_SENHA as prazoTrocaSenha,
                    DT_INCLUSAO AS dataInclusao,
                    ID_ATENDENTE_ALTERACAO AS idAtendenteAlteracao,
                    DT_ALTERACAO AS dataAlteracao
                FROM TB_ATENDENTE
                WHERE ID_ATENDENTE = @id";

            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                return await connection.QuerySingleOrDefaultAsync<Atendente>(query, new { id });
            }
        }

        public async Task<Atendente> ConsultarPorEmail(string email)
        {
            var query = @"
                SELECT 
                    ID_ATENDENTE AS idAtendente,
                    ID_SALAO AS idSalao, 
                    NM_ATENDENTE AS nome,
                    NM_CELULAR AS celular,
                    NM_EMAIL AS email,
                    NM_SENHA AS senha,
                    DV_ADMINISTRADOR AS ehAdministrador,
                    DV_STATUS AS status,
                    HR_INICIO_ALMOCO AS InicioAlmoco,
                    HR_FIM_ALMOCO AS TerminoAlmoco,
                    NM_IMAGEM as ImagemAtendente,
                    NM_TOKEN_ALTERA_SENHA as tokenTrocaSenha,
                    DT_PRAZO_ALTERA_SENHA as prazoTrocaSenha,
                    DT_INCLUSAO AS dataInclusao,
                    ID_ATENDENTE_ALTERACAO AS idAtendenteAlteracao,
                    DT_ALTERACAO AS dataAlteracao
                FROM TB_ATENDENTE
                WHERE NM_EMAIL = @email";

            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                await connection.OpenAsync();
                return await connection.QuerySingleOrDefaultAsync<Atendente>(query, new { email });
            }
        }

        private async Task<Atendente> ConsultarPorCelular(string celular)
        {
            var query = @"
                SELECT 
                    ID_ATENDENTE AS idAtendente,
                    ID_SALAO AS idSalao, 
                    NM_ATENDENTE AS nome,
                    NM_CELULAR AS celular,
                    NM_EMAIL AS email,
                    NM_SENHA AS senha,
                    DV_ADMINISTRADOR AS ehAdministrador,
                    DV_STATUS AS status,
                    HR_INICIO_ALMOCO AS InicioAlmoco,
                    HR_FIM_ALMOCO AS TerminoAlmoco,
                    NM_IMAGEM as ImagemAtendente,
                    NM_TOKEN_ALTERA_SENHA as tokenTrocaSenha,
                    DT_PRAZO_ALTERA_SENHA as prazoTrocaSenha,
                    DT_INCLUSAO AS dataInclusao,
                    ID_ATENDENTE_ALTERACAO AS idAtendenteAlteracao,
                    DT_ALTERACAO AS dataAlteracao
                FROM TB_ATENDENTE
                WHERE NM_CELULAR = @celular";

            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                await connection.OpenAsync();
                return await connection.QuerySingleOrDefaultAsync<Atendente>(query, new { celular });
            }
        }

        public async Task<IEnumerable<Atendente>> Listar(Guid idSalao)
        {
            var query = @"
                SELECT 
                    ID_ATENDENTE as IdAtendente,
                    ID_SALAO as IdSalao,
                    NM_ATENDENTE as Nome,
                    NM_CELULAR as Celular,
                    NM_EMAIL as Email,
                    DV_ADMINISTRADOR as EhAdministrador,
                    DV_STATUS as Status,
                    HR_INICIO_ALMOCO AS InicioAlmoco,
                    HR_FIM_ALMOCO AS TerminoAlmoco,
                    NM_IMAGEM as ImagemAtendente,
                    NM_TOKEN_ALTERA_SENHA as tokenTrocaSenha,
                    DT_PRAZO_ALTERA_SENHA as prazoTrocaSenha,
                    DT_INCLUSAO as DataInclusao,
                    ID_ATENDENTE_ALTERACAO as IdAtendenteAlteracao,
                    DT_ALTERACAO as DataAlteracao
                FROM TB_ATENDENTE
                WHERE ID_SALAO = @idSalao";

            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                return await connection.QueryAsync<Atendente>(query, new { idSalao });
            }
        }

        public async Task<RetornoGenerico> Incluir(Atendente obj, string salao)
        {
            var validacao = await ValidarCampos(obj);

            if (!validacao.Sucesso)
                return validacao;

            obj.Senha = ValidateExtensions.GerarSenhaRandom();

            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                await connection.OpenAsync();
                string query = MontandoQuery(obj, true);
                var retornoProc = new DynamicParameters().OutputParameter();
                await connection.ExecuteAsync(query, retornoProc);

                bool sucesso = retornoProc.Get<int>("@cd_retorno") is 0;
                if (!sucesso)
                    return new RetornoGenerico(false, "Não foi possível realizar a requisição, contate o administrador do sistema");
            }

            return new RetornoGenerico(true, "Atendente incluído com sucesso");
        }

        public async Task<RetornoGenerico> Alterar(Atendente obj)
        {
            var validacao = await ValidarCampos(obj);

            if (!validacao.Sucesso)
                return validacao;

            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                await connection.OpenAsync();
                string query = MontandoQuery(obj, false);
                var retornoProc = new DynamicParameters().OutputParameter();
                await connection.ExecuteAsync(query, retornoProc);

                bool sucesso = retornoProc.Get<int>("@cd_retorno") is 0;
                return new RetornoGenerico(sucesso, sucesso ? "Atendente alterado com sucesso" : "Não foi possível realizar a requisição, contate o administrador do sistema");
            }
        }

        private static string MontandoQuery(Atendente obj, bool ehIncluir)
        {
            return SqlParameterExtensions.PrepararProcedure("p_incluir_alterar_tb_atendente", new Dictionary<string, object>
            {
                { "id_atendente", ehIncluir ? null : obj.IdAtendente },
                { "id_salao", obj.IdSalao },
                { "nm_atendente", obj.Nome },
                { "nm_celular", obj.Celular },
                { "nm_email", obj.Email.ToLower() },
                { "nm_senha", obj.Senha },
                { "nm_imagem", obj.ImagemAtendente },
                { "nm_token_altera_senha", obj.TokenTrocaSenha },
                { "dt_prazo_altera_senha", obj.PrazoTrocaSenha },
                { "dv_administrador", obj.EhAdministrador },
                { "dv_status", obj.Status },
                { "hr_inicio_almoco", obj.InicioAlmoco },
                { "hr_fim_almoco", obj.TerminoAlmoco },
                { "id_atendente_alteracao", ehIncluir ? null : obj.IdAtendenteAlteracao }
            });
        }

        public async Task<DadosAtendenteLogadoViewModel> Autenticar(LoginViewModel obj)
        {
            string query = MontarProcedureAutenticacao(obj);
            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                return await connection.QuerySingleOrDefaultAsync<DadosAtendenteLogadoViewModel>(query);
            }
        }

        private static string MontarProcedureAutenticacao(LoginViewModel obj)
        {
            return SqlParameterExtensions.PrepararProcedure("p_autentica_atendente", new Dictionary<string, object>
            {
                { "nm_email", obj.Email },
                { "nm_senha", obj.Senha }
            }, false);
        }

        public ClaimsPrincipal CriarClaims(DadosAtendenteLogadoViewModel obj)
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Role, "interno"));

            if (obj.EhAdministrador)
                claims.Add(new Claim(ClaimTypes.Role, "administrador"));

            claims.Add(new Claim(ClaimTypes.Gender, JsonConvert.SerializeObject(obj)));
            claims.Add(new Claim(ClaimTypes.Expiration, obj.ValidadePlano.ToString()));

            ClaimsIdentity identities = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
            return new ClaimsPrincipal(new[] { identities });
        }

        private async Task<RetornoGenerico> ValidarCampos(Atendente obj)
        {
            var atendentePorEmail = await ConsultarPorEmail(obj.Email);
            var atendentePorCelular = await ConsultarPorCelular(obj.Celular);
            AtendenteValidation validation = new();

            var retorno = validation.Validate(obj ?? new Atendente());

            if (!retorno.IsValid)
                return new RetornoGenerico(false, retorno.Errors.Select(x => x.ErrorMessage).Take(5).ToList());

            if (atendentePorEmail != null && obj.IdAtendente != atendentePorEmail?.IdAtendente)
                return new RetornoGenerico(false, "E-mail já cadastrado, informe um diferente");

            if (atendentePorCelular != null && obj.IdAtendente != atendentePorCelular?.IdAtendente)
                return new RetornoGenerico(false, "Celular já cadastrado, informe um diferente");

            return new RetornoGenerico(true, String.Empty);
        }
    }
}