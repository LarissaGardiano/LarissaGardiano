using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Agendamento.Service.Services;
using Agendamento.Service.Interfaces;
using Agendamento.Service.Context;

namespace Agendamento.Infra.CrossCutting.IoC
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddAgendamentoSetup(this IServiceCollection services)
        {
            AddApplicationSetup(services);
            AddInfraSetup(services);

            return services;
        }
        
        public static void AddApplicationSetup(this IServiceCollection services)
        {
            services
              .AddScoped<ISalaoAppService, SalaoAppService>()
              .AddScoped<IAtendenteAppService, AtendenteAppService>()
              .AddScoped<IExpedienteAppService, ExpedienteAppService>()
              .AddScoped<IServicoAppService, ServicoAppService>()
              .AddScoped<IAgendamentoAppService, AgendamentoAppService>()
              .AddScoped<IHorarioAgendaAppService, HorarioAgendaAppService>();
        }

        public static void AddInfraSetup(this IServiceCollection services)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddDbContext<AgendamentoContext>(ServiceLifetime.Scoped);
        }
    }
}