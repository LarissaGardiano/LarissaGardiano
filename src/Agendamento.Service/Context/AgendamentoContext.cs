using Agendamento.Infra.CrossCutting.Util.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Agendamento.Service.Context
{
    public class AgendamentoContext : DbContext
    {
        private readonly WebSettings _webSettings;

        public AgendamentoContext(IOptions<WebSettings> options)
        {
            _webSettings = options.Value;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(_webSettings.Database.Schema);
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_webSettings.Database.ConnectionString)
                    .EnableSensitiveDataLogging();
            }
        }
    }
}