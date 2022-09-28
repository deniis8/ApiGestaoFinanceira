using ApiGestaoFinanceira.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ApiGestaoFinanceira.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            /*.builder.Entity<Lancamento>()
                 .HasOne(lancamento => lancamento.IdCCusto);
            WithOne(lancamento => lancamento.DescriCCusto)
            .HasForeignKey<Lancamento>(lancamento => lancamento.IdCCusto);*/
        }

        public DbSet<Lancamento> Lancamentos { get; set; }
        public DbSet<CentroCusto> CentroCustos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .Build();

            optionsBuilder.UseSqlServer(configuration.GetConnectionString("ServerConnectionGF"));
        }
    }   
}
