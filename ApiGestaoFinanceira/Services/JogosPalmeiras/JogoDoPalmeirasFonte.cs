using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ApiGestaoFinanceira.Services
{
    /// <summary>
    /// jogodopalmeiras.com: a home lista os próximos jogos (schema.org ItemList) e a página de cada jogo
    /// traz um SportsEvent com as transmissões (BroadcastEvent), ambos em JSON-LD.
    /// </summary>
    public class JogoDoPalmeirasFonte : IFonteJogoPalmeiras
    {
        private static readonly Uri UrlAgenda = new Uri("https://jogodopalmeiras.com/");

        private static readonly Regex BlocoJsonLd = new Regex(
            @"<script[^>]*type=[""']application/ld\+json[""'][^>]*>(.*?)</script>",
            RegexOptions.Singleline | RegexOptions.IgnoreCase);

        private readonly HttpClient _httpClient;

        public JogoDoPalmeirasFonte(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public string Nome => "jogodopalmeiras.com";

        public async Task<JogoEncontrado> BuscaProximoJogo()
        {
            var home = await _httpClient.GetStringAsync(UrlAgenda);

            var urlJogo = ExtraiUrlProximoJogo(home);
            if (urlJogo == null) return null;

            var paginaJogo = await _httpClient.GetStringAsync(urlJogo);

            var evento = EncontraPorTipo(paginaJogo, "SportsEvent");
            if (evento == null) return null;

            return MontaJogo(evento.Value);
        }

        private static Uri ExtraiUrlProximoJogo(string html)
        {
            var lista = EncontraPorTipo(html, "ItemList");
            if (lista == null || !lista.Value.TryGetProperty("itemListElement", out var itens) || itens.ValueKind != JsonValueKind.Array)
                return null;

            foreach (var item in itens.EnumerateArray())
            {
                // A URL vem da página; só segue se continuar no mesmo site.
                if (Uri.TryCreate(item.TextoOuNulo("url"), UriKind.Absolute, out var url)
                    && url.Scheme == Uri.UriSchemeHttps
                    && url.Host == UrlAgenda.Host)
                    return url;
            }
            return null;
        }

        private static JogoEncontrado MontaJogo(JsonElement evento)
        {
            // O startDate vem com o fuso (ex.: 2026-09-20T11:00:00-03:00); interessa o horário de Brasília como está escrito.
            if (!DateTimeOffset.TryParse(evento.TextoOuNulo("startDate"), CultureInfo.InvariantCulture, DateTimeStyles.None, out var inicio))
                return null;

            return new JogoEncontrado
            {
                Data = inicio.DateTime.Date,
                Hora = inicio.DateTime.TimeOfDay,
                Local = evento.TryGetProperty("location", out var local) ? local.TextoOuNulo("name") : null,
                Oponente = ExtraiOponente(evento),
                Emissoras = ExtraiEmissoras(evento)
            };
        }

        private static string ExtraiOponente(JsonElement evento)
        {
            return new[] { "homeTeam", "awayTeam" }
                .Select(lado => evento.TryGetProperty(lado, out var time) ? time.TextoOuNulo("name") : null)
                .FirstOrDefault(nome => !string.IsNullOrWhiteSpace(nome)
                    && !nome.Equals(JogoEncontrado.Palmeiras, StringComparison.OrdinalIgnoreCase));
        }

        private static List<string> ExtraiEmissoras(JsonElement evento)
        {
            if (!evento.TryGetProperty("subjectOf", out var transmissoes))
                return new List<string>();

            // No JSON-LD, uma lista com um único item também pode vir como objeto.
            var itens = transmissoes.ValueKind == JsonValueKind.Array
                ? transmissoes.EnumerateArray().ToList()
                : new List<JsonElement> { transmissoes };

            return itens
                .Select(t => t.TryGetProperty("publishedOn", out var canal) ? canal.TextoOuNulo("name") : null)
                .Where(nome => !string.IsNullOrWhiteSpace(nome))
                .ToList();
        }

        /// <summary>
        /// Percorre todos os blocos JSON-LD da página e devolve o primeiro objeto do tipo pedido, em qualquer nível.
        /// </summary>
        private static JsonElement? EncontraPorTipo(string html, string tipo)
        {
            foreach (Match bloco in BlocoJsonLd.Matches(html))
            {
                try
                {
                    using var documento = JsonDocument.Parse(bloco.Groups[1].Value);
                    var encontrado = BuscaTipo(documento.RootElement, tipo);
                    if (encontrado != null) return encontrado.Value.Clone();
                }
                catch (JsonException)
                {
                    // Bloco JSON-LD inválido não impede de olhar os demais.
                }
            }
            return null;
        }

        private static JsonElement? BuscaTipo(JsonElement elemento, string tipo)
        {
            if (elemento.ValueKind == JsonValueKind.Object)
            {
                if (TemTipo(elemento, tipo)) return elemento;

                foreach (var propriedade in elemento.EnumerateObject())
                {
                    var encontrado = BuscaTipo(propriedade.Value, tipo);
                    if (encontrado != null) return encontrado;
                }
            }
            else if (elemento.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in elemento.EnumerateArray())
                {
                    var encontrado = BuscaTipo(item, tipo);
                    if (encontrado != null) return encontrado;
                }
            }
            return null;
        }

        private static bool TemTipo(JsonElement objeto, string tipo)
        {
            if (!objeto.TryGetProperty("@type", out var valor)) return false;

            return valor.ValueKind == JsonValueKind.Array
                ? valor.EnumerateArray().Any(t => t.ValueKind == JsonValueKind.String && t.GetString() == tipo)
                : valor.ValueKind == JsonValueKind.String && valor.GetString() == tipo;
        }
    }
}
