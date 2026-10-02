using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ApiGestaoFinanceira.Services
{
    /// <summary>
    /// Cupons do Promobit: limpa o cupom informado na oferta e escolhe, entre os cupons gerais da loja
    /// (página /cupons/loja/{loja}/), os que de fato servem para o produto.
    /// </summary>
    public class PromobitCupons
    {
        private const int MaximoCuponsLoja = 3;

        private static readonly TimeSpan TempoCache = TimeSpan.FromHours(1);
        private static readonly TimeSpan TempoCacheFalha = TimeSpan.FromMinutes(5);

        // Uma busca pode envolver dezenas de lojas: limita as consultas simultâneas ao site.
        private static readonly SemaphoreSlim Concorrencia = new SemaphoreSlim(4);

        // Código de verdade: uma "palavra" só, sem espaço (descarta "Resgate no link", "Moedas no APP", base64 com "=").
        private static readonly Regex FormatoCodigo = new Regex(@"^[A-Za-z0-9][A-Za-z0-9_-]{2,29}$");

        // A oferta às vezes traz mais de um código: "TRIBITB5 + BRCD5", "PV5J6QD8AJ8B ou PEY7LA064KFH".
        private static readonly Regex SeparadorCodigos = new Regex(@"\s*(?:\+|,|;|/|\bou\b|\be\b)\s*", RegexOptions.IgnoreCase);

        // Marcadores que aparecem no lugar do código, mas não são algo para digitar no carrinho.
        private static readonly HashSet<string> NaoSaoCodigos = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "CUPOMAUTOMATICO", "AUTOMATICO", "SEMCUPOM", "NAOPRECISA"
        };

        // Cupons que não valem para qualquer comprador ou produto (texto já sem acento e em minúsculas).
        private static readonly string[] Restricoes =
        {
            "selecionad", "participante", "primeir", "novos usuarios", "novo usuario", "novos clientes", "novo cliente"
        };

        // "em celulares", "na linha Lego", "para seu pet": o cupom é de uma categoria ou produto, não da loja toda.
        private static readonly Regex Escopo = new Regex(@"\b(?:em|no|na|nos|nas|para)\s+([a-z0-9]+)");

        // Palavras que podem vir depois de "em/no/na/para" sem restringir o cupom ("em suas compras", "no carrinho").
        // O nome da própria loja também é permitido ("na AliExpress").
        private static readonly HashSet<string> EscoposGerais = new HashSet<string>
        {
            "compra", "compras", "pedido", "pedidos", "seu", "seus", "sua", "suas", "todo", "todos", "toda", "todas",
            "qualquer", "site", "app", "carrinho", "cupom", "codigo", "link", "momento"
        };

        private static readonly Regex CompraMinima = new Regex(
            @"(?:a partir de|acima de|minimo de|minima de|compras? de)\s*r\$\s*(\d{1,3}(?:\.\d{3})+(?:,\d{1,2})?|\d+(?:,\d{1,2})?)");

        private static readonly Regex DescontoFixo = new Regex(@"(?:R|US)\$\s*(\d{1,3}(?:\.\d{3})+|\d+)(?:,\d{1,2})?", RegexOptions.IgnoreCase);
        private static readonly Regex DescontoPercentual = new Regex(@"(\d{1,2}(?:[.,]\d+)?)\s*%");

        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private readonly ILogger<PromobitCupons> _logger;

        public PromobitCupons(HttpClient httpClient, IMemoryCache cache, ILogger<PromobitCupons> logger)
        {
            _httpClient = httpClient;
            _cache = cache;
            _logger = logger;
        }

        /// <summary>
        /// Cupons publicados para a loja (inclusive encerrados, que servem para invalidar o cupom de uma oferta).
        /// Nunca lança: se a página falhar, a oferta só fica sem os cupons da loja.
        /// </summary>
        public async Task<IReadOnlyList<CupomLoja>> CuponsDaLoja(string loja)
        {
            var cacheKey = $"promobit-cupons-{loja}";
            if (_cache.TryGetValue(cacheKey, out IReadOnlyList<CupomLoja> emCache))
                return emCache;

            await Concorrencia.WaitAsync();
            try
            {
                var html = await _httpClient.GetStringAsync($"{PromobitPagina.UrlBase}/cupons/loja/{Uri.EscapeDataString(loja)}/");
                var cupons = ExtraiCupons(PromobitPagina.LePageProps(html));

                _cache.Set(cacheKey, cupons, TempoCache);
                return cupons;
            }
            catch (Exception ex) when (ex is HttpRequestException || ex is TaskCanceledException || ex is JsonException || ex is InvalidOperationException)
            {
                _logger.LogWarning(ex, "Falha ao consultar os cupons da loja {Loja} no Promobit", loja);

                var vazio = new List<CupomLoja>();
                _cache.Set(cacheKey, (IReadOnlyList<CupomLoja>)vazio, TempoCacheFalha);
                return vazio;
            }
            finally
            {
                Concorrencia.Release();
            }
        }

        /// <summary>
        /// Monta os cupons da oferta: primeiro os indicados nela (origem "oferta"), depois até 3 cupons gerais
        /// da loja que cabem no preço (origem "loja"), do maior para o menor desconto.
        /// </summary>
        public static List<CupomEncontrado> MontaCupons(string cupomDaOferta, decimal? preco, IReadOnlyList<CupomLoja> cuponsLoja, DateTimeOffset agora)
        {
            cuponsLoja = cuponsLoja ?? new List<CupomLoja>();
            var porCodigo = cuponsLoja.ToLookup(c => c.Codigo, StringComparer.OrdinalIgnoreCase);
            var cupons = new List<CupomEncontrado>();

            var codigosOferta = ExtraiCodigos(cupomDaOferta);
            foreach (var codigo in codigosOferta)
            {
                var conhecidos = porCodigo[codigo].ToList();
                var ativo = MaisCompleto(conhecidos.Where(c => c.EstaAtivo(agora)));

                // O Promobit já marcou este código como encerrado ou vencido: não adianta mostrar.
                if (conhecidos.Count > 0 && ativo == null) continue;

                cupons.Add(new CupomEncontrado
                {
                    Codigo = codigo,
                    Origem = CupomEncontrado.OrigemOferta,
                    Desconto = ativo?.Desconto,
                    Descricao = ativo?.Descricao,
                    CompraMinima = ativo?.CompraMinima,
                    ValidoAte = ativo?.ValidoAte
                });
            }

            var daLoja = cuponsLoja
                .Where(c => c.EstaAtivo(agora) && c.Geral && !codigosOferta.Contains(c.Codigo, StringComparer.OrdinalIgnoreCase))
                // O mesmo código costuma estar publicado mais de uma vez; fica a versão com mais informação.
                .GroupBy(c => c.Codigo, StringComparer.OrdinalIgnoreCase)
                .Select(MaisCompleto)
                .Where(c => CabeNoPreco(c, preco))
                // Códigos diferentes com o mesmo desconto e mínimo (ex.: BRGM2 e MEGABR02) são alternativas iguais: basta um.
                .GroupBy(c => (c.ValorFixo, c.Percentual, c.CompraMinima))
                .Select(MaisCompleto)
                .OrderByDescending(c => c.Economia(preco))
                .Take(MaximoCuponsLoja)
                .Select(c => new CupomEncontrado
                {
                    Codigo = c.Codigo,
                    Origem = CupomEncontrado.OrigemLoja,
                    Desconto = c.Desconto,
                    Descricao = c.Descricao,
                    CompraMinima = c.CompraMinima,
                    ValidoAte = c.ValidoAte
                });

            cupons.AddRange(daLoja);
            return cupons;
        }

        public static List<string> ExtraiCodigos(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto)) return new List<string>();

            return SeparadorCodigos.Split(texto.Trim())
                .Select(c => c.Trim())
                .Where(c => FormatoCodigo.IsMatch(c) && !NaoSaoCodigos.Contains(c))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        // Todo cupom geral tem compra mínima; sem o preço do produto não dá para conferir.
        private static bool CabeNoPreco(CupomLoja cupom, decimal? preco) => preco.HasValue && cupom.CompraMinima <= preco;

        private static CupomLoja MaisCompleto(IEnumerable<CupomLoja> cupons)
        {
            return cupons
                .OrderByDescending(c => (c.CompraMinima.HasValue ? 1 : 0) + (c.ValidoAte.HasValue ? 1 : 0))
                .ThenByDescending(c => c.Publicacao ?? DateTimeOffset.MinValue)
                .FirstOrDefault();
        }

        private static List<CupomLoja> ExtraiCupons(JsonElement pageProps)
        {
            if (!pageProps.TryGetProperty("serverCoupons", out var serverCoupons)
                || !serverCoupons.TryGetProperty("coupons", out var lista)
                || lista.ValueKind != JsonValueKind.Array)
                throw new InvalidOperationException("Lista de cupons não encontrada no JSON da página do Promobit.");

            return lista.EnumerateArray()
                .Select(MontaCupomLoja)
                .Where(c => c != null)
                .ToList();
        }

        private static CupomLoja MontaCupomLoja(JsonElement cupom)
        {
            var codigo = cupom.TextoOuNulo("couponCode")?.Trim();
            if (string.IsNullOrEmpty(codigo) || !FormatoCodigo.IsMatch(codigo) || NaoSaoCodigos.Contains(codigo))
                return null;

            var titulo = cupom.TextoOuNulo("couponTitle")?.Trim();
            var instrucoes = cupom.TextoOuNulo("couponInstructions")?.Trim();
            var desconto = cupom.TextoOuNulo("couponDiscountShort")?.Trim();
            // Alguns cupons vêm rotulados "US$ 18 OFF" com o texto dizendo "R$ 18 de Desconto": vale o texto.
            if (desconto != null && desconto.StartsWith("US$", StringComparison.OrdinalIgnoreCase) && titulo != null && titulo.Contains("R$"))
                desconto = "R$" + desconto.Substring(3);
            var texto = SemAcento($"{titulo} {instrucoes}").ToLowerInvariant();
            var compraMinima = LeValor(CompraMinima.Match(texto));

            return new CupomLoja
            {
                Codigo = codigo,
                Ativo = cupom.TextoOuNulo("couponStatusName") == "APPROVED",
                Desconto = string.IsNullOrEmpty(desconto) ? null : desconto,
                Descricao = string.IsNullOrEmpty(titulo) ? null : titulo,
                CompraMinima = compraMinima,
                ValidoAte = cupom.DataOuNula("couponUntil"),
                Publicacao = cupom.DataOuNula("couponPublished"),
                // Só é sugerido para qualquer produto o cupom que declara a compra mínima e não restringe categoria nem comprador.
                // Os cupons de categoria ("10% OFF em Eletrodomésticos") são a maioria e não dá para casar com o produto com segurança.
                Geral = compraMinima.HasValue
                    && !Restricoes.Any(texto.Contains)
                    && !TemEscopoEspecifico(texto, cupom.TextoOuNulo("storeName")),
                ValorFixo = desconto == null ? null : LeValor(DescontoFixo.Match(desconto)),
                Percentual = desconto == null ? null : LeValor(DescontoPercentual.Match(desconto))
            };
        }

        private static bool TemEscopoEspecifico(string texto, string loja)
        {
            var palavrasLoja = SemAcento(loja ?? "").ToLowerInvariant().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            return Escopo.Matches(texto)
                .Select(m => m.Groups[1].Value)
                // "em R$ 50" / "acima de R$ 90": valor, não categoria.
                .Where(palavra => palavra != "r" && !char.IsDigit(palavra[0]))
                .Any(palavra => !EscoposGerais.Contains(palavra) && !palavrasLoja.Contains(palavra));
        }

        // "1.300" e "1.300,00" viram 1300; "99,90" vira 99.9.
        private static decimal? LeValor(Match match)
        {
            if (!match.Success) return null;

            var numero = match.Groups[1].Value.Replace(".", "").Replace(",", ".");
            return decimal.TryParse(numero, NumberStyles.Number, CultureInfo.InvariantCulture, out var valor) ? valor : (decimal?)null;
        }

        private static string SemAcento(string texto)
        {
            var semAcento = texto.Normalize(NormalizationForm.FormD)
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark);

            return new string(semAcento.ToArray()).Normalize(NormalizationForm.FormC);
        }
    }

    public class CupomLoja
    {
        public string Codigo { get; set; }
        public bool Ativo { get; set; }
        public string Desconto { get; set; }
        public string Descricao { get; set; }
        public decimal? CompraMinima { get; set; }
        public DateTimeOffset? ValidoAte { get; set; }
        public DateTimeOffset? Publicacao { get; set; }
        /// <summary>Vale para qualquer produto da loja acima da compra mínima.</summary>
        public bool Geral { get; set; }
        public decimal? ValorFixo { get; set; }
        public decimal? Percentual { get; set; }

        public bool EstaAtivo(DateTimeOffset agora) => Ativo && (!ValidoAte.HasValue || ValidoAte > agora);

        /// <summary>Quanto o cupom economiza no produto, para ordenar; frete grátis e brindes ficam por último.</summary>
        public decimal Economia(decimal? preco)
        {
            if (ValorFixo.HasValue) return ValorFixo.Value;
            if (Percentual.HasValue && preco.HasValue) return preco.Value * Percentual.Value / 100;
            return 0;
        }
    }
}
