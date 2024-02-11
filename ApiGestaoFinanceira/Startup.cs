using ApiGestaoFinanceira.Data;
using ApiGestaoFinanceira.Services;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;

namespace ApiGestaoFinanceira
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            /*Console.WriteLine("Escolha um ambiente: ");
            Console.WriteLine("01 - Prod");
            Console.WriteLine("02 - Dev");
            string opc = Console.ReadLine();
            string ambiente = "";

            if(opc == "01")
            {
                ambiente = "ServerConnectionGF";
            }
            else if(opc == "02")
            {
                ambiente = "ServerConnectionGFDev";
            }*/
            string ambiente = "ServerConnectionGFOrangePi";
            //string ambiente = "ServerConnectionGFDev";

            services.AddDbContextPool<AppDbContext>(options => options
                   .UseMySql(
                       Configuration.GetConnectionString(ambiente),
                       mySqlOptions => mySqlOptions.ServerVersion(new Version(10, 6, 10), ServerType.MariaDb)
                   )
               );

            services.AddControllers();
            //services.AddDbContext<AppDbContext>(option => option.UseSqlServer(Configuration.GetConnectionString("ServerConnectionGF")));
            //services.AddDbContext<AppDbContext>(option => option.UseMySql(Configuration.GetConnectionString("ServerConnectionGF")));
           
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<LancamentoService, LancamentoService>();
            services.AddScoped<CentroCustoService, CentroCustoService>();
            services.AddScoped<UsuarioService, UsuarioService>();
            services.AddScoped<GastosMensaisService, GastosMensaisService>();
            services.AddScoped<SaldosInvestimentosService, SaldosInvestimentosService>();
            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            /*
            else
            {
                app.UseHsts();
            }*/

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseCors(
        options => options.WithOrigins("http://localhost:8100").AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin() //AllowAnyOrigin().AllowAnyHeader()
    );

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
