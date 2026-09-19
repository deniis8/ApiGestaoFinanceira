using ApiGestaoFinanceira.Data.Dto.JogosPalmeiras;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApiGestaoFinanceira.Services
{
    /// <summary>
    /// Busca o próximo jogo do Palmeiras nas fontes de agenda e grava (upsert) no Supabase.
    /// </summary>
    public class SincronizaJogoPalmeirasService
    {
        // Horário de Brasília não tem mais horário de verão (extinto em 2019), então o deslocamento é fixo.
        private static readonly TimeSpan DeslocamentoBrasilia = TimeSpan.FromHours(-3);

        private readonly ProximoJogoService _proximoJogoService;
        private readonly PartidaFutebolSupabaseRepositorio _repositorio;
        private readonly ILogger<SincronizaJogoPalmeirasService> _logger;

        public SincronizaJogoPalmeirasService(
            ProximoJogoService proximoJogoService,
            PartidaFutebolSupabaseRepositorio repositorio,
            ILogger<SincronizaJogoPalmeirasService> logger)
        {
            _proximoJogoService = proximoJogoService;
            _repositorio = repositorio;
            _logger = logger;
        }

        /// <summary>
        /// Retorna a partida gravada, ou null quando não há o que gravar (sem jogo agendado ou sem horário definido).
        /// Lança exceção quando as fontes ou o Supabase não respondem, para quem chama tentar de novo.
        /// </summary>
        public async Task<PartidaFutebol> Sincroniza(CancellationToken cancellationToken = default)
        {
            var jogo = await _proximoJogoService.RecuperaProximoJogo();
            if (jogo == null)
            {
                _logger.LogInformation("Nenhum jogo do Palmeiras agendado; nada a sincronizar.");
                return null;
            }

            // Sem horário a data vem sozinha ("yyyy-MM-dd"). Gravar meia-noite seria inventar um horário
            // e criaria uma linha duplicada quando o horário real fosse definido.
            if (!DateTime.TryParseExact(jogo.DataHora, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var inicio))
            {
                _logger.LogWarning("Jogo {Jogo} sem horário definido ({DataHora}); não foi sincronizado.", jogo.Jogo, jogo.DataHora);
                return null;
            }

            var emissoras = (jogo.OndeAssistir ?? Enumerable.Empty<ReadEmissoraDto>())
                .Select(e => e.Emissora)
                .Where(nome => !string.IsNullOrWhiteSpace(nome))
                .ToList();

            var partida = new PartidaFutebol
            {
                Jogo = jogo.Jogo,
                Local = string.IsNullOrWhiteSpace(jogo.Local) ? null : jogo.Local,
                DataHora = new DateTimeOffset(inicio, DeslocamentoBrasilia).ToString("yyyy-MM-dd'T'HH:mm:sszzz", CultureInfo.InvariantCulture),
                Emissoras = emissoras.Count > 0 ? string.Join("|", emissoras) : null
            };

            await _repositorio.Upsert(partida, cancellationToken);

            _logger.LogInformation("Jogo sincronizado com o Supabase: {Jogo} em {DataHora}.", partida.Jogo, partida.DataHora);
            return partida;
        }
    }
}
