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
           
            string ambiente = "ServerConnectionGFOrangePi";
            //string ambiente = "ServerConnectionGFDev";

            services.AddDbContextPool<AppDbContext>(options => options
                   .UseMySql(
                       Configuration.GetConnectionString(ambiente),
                       mySqlOptions => mySqlOptions.ServerVersion(new Version(10, 6, 10), ServerType.MariaDb)
                   )
               );

            services.AddControllers();
           
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<LancamentoService, LancamentoService>();
            services.AddScoped<CentroCustoService, CentroCustoService>();
            services.AddScoped<UsuarioService, UsuarioService>();
            services.AddScoped<GastosMensaisService, GastosMensaisService>();
            services.AddScoped<SaldosInvestimentosService, SaldosInvestimentosService>();
            services.AddScoped<GastosCentroCustoService, GastosCentroCustoService>();
            services.AddScoped<DetalhamentoGastosCentroCustoService, DetalhamentoGastosCentroCustoService>();
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
