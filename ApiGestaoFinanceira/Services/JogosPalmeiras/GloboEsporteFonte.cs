using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApiGestaoFinanceira.Services
{
    /// <summary>
    /// ge.globo.com: a página da agenda embute os jogos como um objeto JSON dentro de um script ("scheduleTeam: {...}").
    /// </summary>
    public class GloboEsporteFonte : IFonteJogoPalmeiras
    {
        private const string UrlAgenda = "https://ge.globo.com/futebol/times/palmeiras/agenda-de-jogos-do-palmeiras/";
        private const string MarcadorJson = "scheduleTeam:";

        // O "Cartola" aparece junto das fontes de transmissão, mas é um jogo de fantasy, não uma emissora.
        private static readonly HashSet<string> EmissorasIgnoradas = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Cartola" };

        private readonly HttpClient _httpClient;

        public GloboEsporteFonte(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public string Nome => "ge.globo.com";

        public async Task<JogoEncontrado> BuscaProximoJogo()
        {
            var html = await _httpClient.GetStringAsync(UrlAgenda);

            using var documento = JsonDocument.Parse(ExtraiJsonAgenda(html));
            var agenda = documento.RootElement.GetProperty("teamAgenda");

            var partida = PrimeiraPartida(agenda, "now") ?? PrimeiraPartida(agenda, "future");
            if (partida == null) return null;

            var jogo = partida.Value;

            if (!DateTime.TryParseExact(jogo.TextoOuNulo("startDate"), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var data))
                return null;

            return new JogoEncontrado
            {
                Data = data,
                Hora = TimeSpan.TryParse(jogo.TextoOuNulo("startHour"), CultureInfo.InvariantCulture, out var hora) ? hora : (TimeSpan?)null,
                Local = jogo.TryGetProperty("location", out var local) ? local.TextoOuNulo("popularName") : null,
                Oponente = ExtraiOponente(jogo),
                Emissoras = ExtraiEmissoras(jogo)
            };
        }

        private static JsonElement? PrimeiraPartida(JsonElement agenda, string grupo)
        {
            if (!agenda.TryGetProperty(grupo, out var lista) || lista.ValueKind != JsonValueKind.Array)
                return null;

            foreach (var evento in lista.EnumerateArray())
            {
                if (evento.TryGetProperty("match", out var partida) && partida.ValueKind == JsonValueKind.Object)
                    return partida.Clone();
            }
            return null;
        }

        private static string ExtraiOponente(JsonElement partida)
        {
            return new[] { "firstContestant", "secondContestant" }
                .Select(lado => partida.TryGetProperty(lado, out var time) ? time.TextoOuNulo("popularName") : null)
                .FirstOrDefault(nome => !string.IsNullOrWhiteSpace(nome)
                    && !nome.Equals(JogoEncontrado.Palmeiras, StringComparison.OrdinalIgnoreCase));
        }

        private static List<string> ExtraiEmissoras(JsonElement partida)
        {
            if (!partida.TryGetProperty("liveWatchSources", out var fontes) || fontes.ValueKind != JsonValueKind.Array)
                return new List<string>();

            return fontes.EnumerateArray()
                .Select(f => f.TextoOuNulo("name"))
                .Where(nome => !string.IsNullOrWhiteSpace(nome) && !EmissorasIgnoradas.Contains(nome))
                .ToList();
        }

        /// <summary>
        /// Localiza o marcador e devolve o trecho do objeto JSON, respeitando chaves que aparecem dentro de strings.
        /// </summary>
        private static string ExtraiJsonAgenda(string html)
        {
            var indiceMarcador = html.IndexOf(MarcadorJson, StringComparison.Ordinal);
            if (indiceMarcador < 0)
                throw new InvalidOperationException("Agenda do Palmeiras não encontrada na página (layout do site pode ter mudado).");

            var inicio = html.IndexOf('{', indiceMarcador);
            if (inicio < 0)
                throw new InvalidOperationException("JSON da agenda do Palmeiras não encontrado na página.");

            var profundidade = 0;
            var dentroDeString = false;
            var escapado = false;

            for (var i = inicio; i < html.Length; i++)
            {
                var c = html[i];

                if (dentroDeString)
                {
                    if (escapado) escapado = false;
                    else if (c == '\\') escapado = true;
                    else if (c == '"') dentroDeString = false;
                    continue;
                }

                if (c == '"') dentroDeString = true;
                else if (c == '{') profundidade++;
                else if (c == '}' && --profundidade == 0)
                    return html.Substring(inicio, i - inicio + 1);
            }

            throw new InvalidOperationException("JSON da agenda do Palmeiras está incompleto.");
        }
    }
}
