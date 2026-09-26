using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApiGestaoFinanceira.Services
{
    /// <summary>
    /// Sincroniza o próximo jogo do Palmeiras com o Supabase em horários fixos do dia (3 por padrão).
    /// Nunca derruba a API: qualquer falha vira log e a próxima execução tenta de novo.
    /// </summary>
    public class SincronizacaoJogosBackgroundService : BackgroundService
    {
        private static readonly TimeSpan[] HorariosPadrao =
        {
            TimeSpan.FromHours(7),
            TimeSpan.FromHours(13),
            TimeSpan.FromHours(19)
        };

        private const int MaximoTentativas = 3;
        private static readonly TimeSpan EsperaEntreTentativas = TimeSpan.FromMinutes(5);

        private readonly IServiceScopeFactory _scopeFactory;
        private readonly SupabaseOptions _supabase;
        private readonly SincronizacaoJogosOptions _opcoes;
        private readonly ILogger<SincronizacaoJogosBackgroundService> _logger;

        public SincronizacaoJogosBackgroundService(
            IServiceScopeFactory scopeFactory,
            IOptions<SupabaseOptions> supabase,
            IOptions<SincronizacaoJogosOptions> opcoes,
            ILogger<SincronizacaoJogosBackgroundService> logger)
        {
            _scopeFactory = scopeFactory;
            _supabase = supabase.Value;
            _opcoes = opcoes.Value;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Não deixa o início da API esperar por esta rotina.
            await Task.Yield();

            if (!_supabase.Configurado)
            {
                _logger.LogWarning("Sincronização de jogos desativada: configure Supabase:Url e Supabase:ServiceRoleKey.");
                return;
            }

            var horarios = LeHorarios(_opcoes.Horarios);

            try
            {
                if (_opcoes.ExecutarNaInicializacao)
                    await Executa(stoppingToken);

                while (!stoppingToken.IsCancellationRequested)
                {
                    var agora = DateTime.Now;
                    var proxima = ProximaExecucao(agora, horarios);
                    _logger.LogInformation("Próxima sincronização do jogo do Palmeiras: {Proxima:dd/MM/yyyy HH:mm}.", proxima);

                    await Task.Delay(proxima - agora, stoppingToken);
                    await Executa(stoppingToken);
                }
            }
            catch (OperationCanceledException)
            {
                // API encerrando.
            }
        }

        private async Task Executa(CancellationToken cancellationToken)
        {
            for (var tentativa = 1; tentativa <= MaximoTentativas; tentativa++)
            {
                try
                {
                    using var escopo = _scopeFactory.CreateScope();
                    var sincronizacao = escopo.ServiceProvider.GetRequiredService<SincronizaJogoPalmeirasService>();
                    await sincronizacao.Sincroniza(cancellationToken);
                    return;
                }
                catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Falha ao sincronizar o jogo do Palmeiras (tentativa {Tentativa} de {Maximo}).", tentativa, MaximoTentativas);

                    if (tentativa < MaximoTentativas)
                        await Task.Delay(EsperaEntreTentativas, cancellationToken);
                }
            }
        }

        private IReadOnlyList<TimeSpan> LeHorarios(string[] configurados)
        {
            var validos = (configurados ?? new string[0])
                .Select(texto => TimeSpan.TryParseExact(texto?.Trim(), @"hh\:mm", CultureInfo.InvariantCulture, out var horario) ? horario : (TimeSpan?)null)
                .Where(horario => horario.HasValue)
                .Select(horario => horario.Value)
                .Distinct()
                .OrderBy(horario => horario)
                .ToList();

            if (validos.Count > 0) return validos;

            if (configurados != null && configurados.Length > 0)
                _logger.LogWarning("Nenhum horário válido em SincronizacaoJogos:Horarios (use HH:mm); usando 07:00, 13:00 e 19:00.");

            return HorariosPadrao;
        }

        private static DateTime ProximaExecucao(DateTime agora, IReadOnlyList<TimeSpan> horarios)
        {
            foreach (var horario in horarios)
            {
                var candidato = agora.Date + horario;
                if (candidato > agora) return candidato;
            }
            return agora.Date.AddDays(1) + horarios[0];
        }
    }
}
