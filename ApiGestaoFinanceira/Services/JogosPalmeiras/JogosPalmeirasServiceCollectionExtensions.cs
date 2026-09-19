using Microsoft.Extensions.Configuration;
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

            // Sincronização periódica do próximo jogo com o Supabase.
            services.AddOptions<SupabaseOptions>()
                .Configure<IConfiguration>((opcoes, configuracao) => configuracao.GetSection("Supabase").Bind(opcoes));
            services.AddOptions<SincronizacaoJogosOptions>()
                .Configure<IConfiguration>((opcoes, configuracao) => configuracao.GetSection("SincronizacaoJogos").Bind(opcoes));

            services.AddHttpClient<PartidaFutebolSupabaseRepositorio>(cliente => cliente.Timeout = TimeSpan.FromSeconds(15));
            services.AddScoped<SincronizaJogoPalmeirasService>();
            services.AddHostedService<SincronizacaoJogosBackgroundService>();

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
