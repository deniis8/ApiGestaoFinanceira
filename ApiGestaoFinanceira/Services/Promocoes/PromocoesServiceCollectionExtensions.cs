using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

namespace ApiGestaoFinanceira.Services
{
    public static class PromocoesServiceCollectionExtensions
    {
        public static IServiceCollection AddPromocoes(this IServiceCollection services)
        {
            services.AddMemoryCache();

            services.AddHttpClient<PromobitCupons>(ConfiguraCliente);
            services.AddHttpClient<PromobitFonte>(ConfiguraCliente);

            // Para adicionar outro site de promoções, basta implementar IFontePromocoes e registrar aqui.
            services.AddTransient<IFontePromocoes>(sp => sp.GetRequiredService<PromobitFonte>());

            services.AddScoped<PromocoesService>();

            return services;
        }

        private static void ConfiguraCliente(HttpClient cliente)
        {
            // Timeout curto para uma página lenta não travar a resposta das outras.
            cliente.Timeout = TimeSpan.FromSeconds(10);
            cliente.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; ApiGestaoFinanceira)");
        }
    }
}
