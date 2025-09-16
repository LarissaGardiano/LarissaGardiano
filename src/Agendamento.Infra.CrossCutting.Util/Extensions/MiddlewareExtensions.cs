using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace Agendamento.Infra.CrossCutting.Util.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseLoggerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoggerMiddleware>();
        }

        public class LoggerMiddleware
        {
            private readonly ILogger<LoggerMiddleware> _logger;
            private readonly RequestDelegate _next;
            private readonly IConfiguration _configuration;

            public LoggerMiddleware(
                RequestDelegate next,
                ILogger<LoggerMiddleware> logger,
                IConfiguration configuration)
            {
                _next = next;
                _logger = logger;
                _configuration = configuration;
            }

            public async Task Invoke(HttpContext context)
            {
                try
                {
                    await _next.Invoke(context);
                }
                catch (Exception ex)
                {
                    var response = context.Response;
                    response.ContentType = "application/json";

                    _logger.LogError($"ERRO: {JsonConvert.SerializeObject(ex)}");

                    if (ex.InnerException.Message.Contains("DELETE conflitou"))
                    {
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        await response.WriteAsync(JsonConvert.SerializeObject(new RetornoGenerico(false, "Não foi possível remover o registro, pois ele esta associado a outro(s) registro(s).")));
                    }
                    else if (ex.InnerException.Message.Contains("host conectado não respondeu"))
                    {
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        await response.WriteAsync(JsonConvert.SerializeObject(new RetornoGenerico(false, "Servidor de destino recusou a solicitação.")));
                    }
                    else
                    {
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        await response.WriteAsync(JsonConvert.SerializeObject(new RetornoGenerico(false, "Erro ao executar solicitação, Contate o administrador do sistema.")));
                    }

                }
            }
        }
    }
}