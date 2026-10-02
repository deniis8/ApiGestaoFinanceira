using System;
using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ApiGestaoFinanceira.Services
{
    /// <summary>
    /// promobit.com.br é feito em Next.js: cada página embute seus dados no JSON do script __NEXT_DATA__.
    /// </summary>
    internal static class PromobitPagina
    {
        public const string UrlBase = "https://www.promobit.com.br";

        private static readonly Regex BlocoNextData = new Regex(
            @"<script[^>]*id=[""']__NEXT_DATA__[""'][^>]*>(.*?)</script>",
            RegexOptions.Singleline | RegexOptions.IgnoreCase);

        /// <summary>
        /// Devolve o props.pageProps da página, ou lança InvalidOperationException se o layout mudou.
        /// </summary>
        public static JsonElement LePageProps(string html)
        {
            var bloco = BlocoNextData.Match(html);
            if (!bloco.Success)
                throw new InvalidOperationException("Dados não encontrados na página do Promobit (layout do site pode ter mudado).");

            using var documento = JsonDocument.Parse(bloco.Groups[1].Value);

            if (!documento.RootElement.TryGetProperty("props", out var props) || !props.TryGetProperty("pageProps", out var pageProps))
                throw new InvalidOperationException("pageProps não encontrado no JSON da página do Promobit.");

            return pageProps.Clone();
        }

        public static decimal? NumeroOuNulo(this JsonElement objeto, string propriedade)
        {
            return objeto.ValueKind == JsonValueKind.Object
                && objeto.TryGetProperty(propriedade, out var valor)
                && valor.ValueKind == JsonValueKind.Number
                && valor.TryGetDecimal(out var numero)
                    ? numero
                    : (decimal?)null;
        }

        /// <summary>As datas vêm com o fuso sem dois-pontos (ex.: 2026-10-01T19:21:00-0300).</summary>
        public static DateTimeOffset? DataOuNula(this JsonElement objeto, string propriedade)
        {
            return DateTimeOffset.TryParse(objeto.TextoOuNulo(propriedade), CultureInfo.InvariantCulture, DateTimeStyles.None, out var data)
                ? data
                : (DateTimeOffset?)null;
        }
    }
}
