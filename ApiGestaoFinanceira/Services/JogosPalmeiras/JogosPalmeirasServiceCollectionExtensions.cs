using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

namespace ApiGestaoFinanceira.Services
{
    public static class JogosPalmeirasServiceCollectionExtensions
    {
        public static IServiceCollection AddJogosPalmeiras(this IServiceCollection services)
        {
            services.AddMemoryCache();

            services.AddHttpClient<GloboEsporteFonte>(ConfiguraCliente);
            services.AddHttpClient<JogoDoPalmeirasFonte>(ConfiguraCliente);

            // A ordem de registro é a prioridade: a primeira fonte define qual é o jogo (ver ProximoJogoService).
            services.AddTransient<IFonteJogoPalmeiras>(sp => sp.GetRequiredService<GloboEsporteFonte>());
            services.AddTransient<IFonteJogoPalmeiras>(sp => sp.GetRequiredService<JogoDoPalmeirasFonte>());

            services.AddScoped<ProximoJogoService>();

            return services;
        }

        private static void ConfiguraCliente(HttpClient cliente)
        {
            // Timeout curto para uma fonte lenta não travar a resposta das outras.
            cliente.Timeout = TimeSpan.FromSeconds(10);
            cliente.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; ApiGestaoFinanceira)");
        }
    }
}
