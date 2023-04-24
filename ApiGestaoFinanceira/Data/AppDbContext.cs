using ApiGestaoFinanceira.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ApiGestaoFinanceira.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Lancamento> Lancamentos { get; set; }
        public DbSet<CentroCusto> CentroCustos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<GastosMensais> GastosMensais { get; set; }

        public DbSet<SaldosInvestimentos> SaldosInvestimentos { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {

        }

        /*
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Lancamento>()
            .HasOne(l => l.CentroCusto)
            .WithMany(c => c.Lancamentos);
        }
        */


        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .Build();

            //optionsBuilder.UseSqlServer(configuration.GetConnectionString("ServerConnectionGF"));
            optionsBuilder.UseMySql(configuration.GetConnectionString("ServerConnectionGF"));
        }

       */
    }
}
