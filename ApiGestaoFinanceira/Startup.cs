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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using ApiGestaoFinanceira.Models;

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
           
            //string ambiente = "ServerConnectionGFOrangePi";
            string ambiente = "ServerConnectionGFDev";

            services.AddDbContextPool<AppDbContext>(options => options
                   .UseMySql(
                       Configuration.GetConnectionString(ambiente),
                       mySqlOptions => mySqlOptions.ServerVersion(new Version(10, 6, 10), ServerType.MariaDb)
                   )
               );

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "gestao-financeira",
                    ValidAudience = "clientes-gestao",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("aplicacaogestaofinanceira@123")),
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddControllers(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new Microsoft.AspNetCore.Mvc.Authorization.AuthorizeFilter(policy));
            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<LancamentoService, LancamentoService>();
            services.AddScoped<CentroCustoService, CentroCustoService>();
            services.AddScoped<UsuarioService, UsuarioService>();
            services.AddScoped<GastosMensaisService, GastosMensaisService>();
            services.AddScoped<SaldosInvestimentosService, SaldosInvestimentosService>();
            services.AddScoped<GastosCentroCustoService, GastosCentroCustoService>();
            services.AddScoped<DetalhamentoGastosCentroCustoService, DetalhamentoGastosCentroCustoService>();
            services.AddScoped<LancamentoFixoService, LancamentoFixoService>();
            services.AddScoped<TokenService>();
            services.AddScoped<LancamentosRecebidosIAService, LancamentosRecebidosIAService>();
            services.AddScoped<GastosCentroCustoIAService, GastosCentroCustoIAService>();
            services.AddScoped<InvestimentosIAService, InvestimentosIAService>();
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

            // ---------------------------------------------------------------------------
            //app.UseHttpsRedirection();
            // ---------------------------------------------------------------------------
            // Ajustes realizados para hospedar a aplicação Angular de forma estática
            // na OrangePi utilizando NGINX, sem depender do Node.js ou node_modules:
            //
            // 1. Geramos o build de produção do Angular usando:
            //      ng build --configuration production
            //    Isso gera os arquivos estáticos (HTML, JS, CSS) em:
            //      /dist/gestao-financeira
            //
            // 2. Copiamos o build para o diretório servido pelo NGINX:
            //      /var/www/gestao-financeira
            //    Dessa forma, o Angular é servido como conteúdo estático,
            //    dispensando a execução do 'ng serve' e o uso de node_modules.
            //
            // 3. Configuramos o NGINX para:
            //      - Servir o Angular no root (/) da aplicação.
            //      - Fazer proxy das chamadas de API (/api/) para a API .NET
            //        rodando na porta 5000, sem expor o Angular ao Node.js.
            //
            // 4. Na API .NET, comentamos a linha:
            //      app.UseHttpsRedirection();
            //    Isso evita redirecionamento automático para HTTPS, que
            //    poderia gerar erros quando a aplicação é acessada via HTTP
            //    do NGINX (proxy reverso), mantendo a compatibilidade com
            //    chamadas do Angular hospedado.
            //
            // Resultado:
            // - Angular funciona via NGINX como site estático.
            // - API .NET é acessível via proxy /api/.
            // - Node.js e node_modules não são mais necessários na OrangePi.
            // - Redução significativa de uso de disco e simplificação da stack.
            //
            // Observação:
            // Esse setup permite que você mantenha a aplicação leve,
            // confiável e de fácil manutenção em dispositivos de baixa memória
            // como a OrangePi.
            //
            // ---------------------------------------------------------------------------            

            app.UseRouting();            

            app.UseCors(options => options
            .WithOrigins("http://192.168.1.110:4200", "http://192.168.1.141:4200", "http://localhost:4200", "http://localhost:8100")  // Permitir Angular
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            );

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
