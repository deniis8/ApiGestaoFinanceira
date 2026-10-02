using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApiGestaoFinanceira.Services
{
    /// <summary>
    /// promobit.com.br: as páginas de categoria e de loja embutem as ofertas no JSON do Next.js
    /// (props.pageProps.serverOffers.offers). As lojas chinesas são consultadas pela página da loja
    /// e cada oferta entra no tipo correspondente à sua categoria.
    /// </summary>
    public class PromobitFonte : IFontePromocoes
    {
        private const string UrlImagens = "https://i.promobit.com.br/300";

        // Páginas de categoria do Promobit que compõem cada tipo.
        private static readonly Dictionary<TipoPromocao, string[]> Categorias = new Dictionary<TipoPromocao, string[]>
        {
            [TipoPromocao.Tecnologia] = new[]
            {
                "informatica",
                "smartphones-tablets-e-telefones",
                "eletronicos-audio-e-video",
                "games",
                "cameras-filmadoras-e-drones"
            },
            [TipoPromocao.Moveis] = new[] { "moveis-e-decoracao" },
            [TipoPromocao.TenisERoupas] = new[] { "moda-e-calcados-masculinos", "moda-e-calcados-femininos" },
            [TipoPromocao.Viagens] = new[] { "viagem" },
            [TipoPromocao.Outros] = new[] { "outros" }
        };

        // Categoria do Promobit -> tipo. O que não está mapeado (beleza, eletrodomésticos...) é "Outros".
        private static readonly Dictionary<string, TipoPromocao> TipoPorCategoria = Categorias
            .SelectMany(c => c.Value.Select(categoria => (Categoria: categoria, Tipo: c.Key)))
            .ToDictionary(c => c.Categoria, c => c.Tipo);

        // Slugs das lojas no Promobit (/promocoes/loja/{slug}/). A Shopee é de Singapura, mas vende
        // sobretudo de vendedores chineses e entra no mesmo grupo.
        private static readonly HashSet<string> LojasChinesas = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "aliexpress", "shein", "shopee", "temu", "tiktok-shop"
        };

        // Oferta que o Promobit já marcou como encerrada (as páginas de loja trazem muitas).
        private const string StatusEncerrada = "FINISHED";

        private readonly HttpClient _httpClient;
        private readonly PromobitCupons _cupons;

        public PromobitFonte(HttpClient httpClient, PromobitCupons cupons)
        {
            _httpClient = httpClient;
            _cupons = cupons;
        }

        public string Nome => "promobit.com.br";

        public async Task<List<PromocaoEncontrada>> BuscaPromocoes(TipoPromocao tipo)
        {
            var urls = Categorias[tipo].Select(c => $"{PromobitPagina.UrlBase}/promocoes/{c}/")
                .Concat(LojasChinesas.Select(l => $"{PromobitPagina.UrlBase}/promocoes/loja/{l}/"))
                .ToList();
            // "Outros" também aproveita o feed da home.
            if (tipo == TipoPromocao.Outros)
                urls.Add($"{PromobitPagina.UrlBase}/");

            var consultas = await Task.WhenAll(urls.Select(ConsultaPagina));

            // Uma página fora do ar não derruba as outras; só falha se nenhuma respondeu.
            if (consultas.All(c => c.Falha != null))
                throw new InvalidOperationException($"Nenhuma página do Promobit respondeu para {tipo}.", consultas[0].Falha);

            var ofertas = consultas
                .Where(c => c.Falha == null)
                .SelectMany(c => c.Ofertas)
                .Where(o => TipoDaCategoria(o.Categoria) == tipo)
                // A mesma oferta aparece na página da categoria e na da loja.
                .GroupBy(o => o.Promocao.Link)
                .Select(g => g.First())
                .ToList();

            var lojas = ofertas.Select(o => o.Loja).Where(l => !string.IsNullOrEmpty(l)).Distinct(StringComparer.OrdinalIgnoreCase);
            var cuponsPorLoja = (await Task.WhenAll(lojas.Select(async l => (Loja: l, Cupons: await _cupons.CuponsDaLoja(l)))))
                .ToDictionary(c => c.Loja, c => c.Cupons, StringComparer.OrdinalIgnoreCase);

            var agora = DateTimeOffset.Now;
            foreach (var oferta in ofertas)
            {
                cuponsPorLoja.TryGetValue(oferta.Loja ?? "", out var cuponsLoja);
                oferta.Promocao.Cupons = PromobitCupons.MontaCupons(oferta.Cupom, oferta.Promocao.Preco, cuponsLoja, agora);
            }

            return ofertas.Select(o => o.Promocao).ToList();
        }

        private static TipoPromocao TipoDaCategoria(string categoria)
        {
            return TipoPorCategoria.TryGetValue(categoria ?? "", out var tipo) ? tipo : TipoPromocao.Outros;
        }

        private async Task<(List<OfertaPromobit> Ofertas, Exception Falha)> ConsultaPagina(string url)
        {
            try
            {
                var html = await _httpClient.GetStringAsync(url);
                return (ExtraiOfertas(PromobitPagina.LePageProps(html)), null);
            }
            catch (Exception ex) when (ex is HttpRequestException || ex is TaskCanceledException || ex is JsonException || ex is InvalidOperationException)
            {
                return (null, ex);
            }
        }

        private static List<OfertaPromobit> ExtraiOfertas(JsonElement pageProps)
        {
            if (!pageProps.TryGetProperty("serverOffers", out var serverOffers)
                || !serverOffers.TryGetProperty("offers", out var ofertas)
                || ofertas.ValueKind != JsonValueKind.Array)
                throw new InvalidOperationException("Lista de ofertas do Promobit não encontrada no JSON da página.");

            return ofertas.EnumerateArray()
                .Where(o => o.TextoOuNulo("offerStatusName") != StatusEncerrada)
                .Select(MontaOferta)
                .Where(o => o != null)
                .ToList();
        }

        private static OfertaPromobit MontaOferta(JsonElement oferta)
        {
            var titulo = oferta.TextoOuNulo("offerTitle");
            var slug = oferta.TextoOuNulo("offerSlug");
            if (string.IsNullOrWhiteSpace(titulo) || string.IsNullOrWhiteSpace(slug))
                return null;

            // O Promobit usa 0 e 0,01 quando não há preço (ex.: "a partir de" ou cupom de loja inteira).
            var preco = oferta.NumeroOuNulo("offerPrice");
            var precoAntigo = oferta.NumeroOuNulo("offerOldPrice");
            if (preco <= 0.01m) preco = null;
            if (precoAntigo <= 0.01m || (preco.HasValue && precoAntigo <= preco)) precoAntigo = null;

            var loja = oferta.TextoOuNulo("storeParam");

            return new OfertaPromobit
            {
                Categoria = oferta.TextoOuNulo("categorySlug"),
                Loja = loja,
                Cupom = oferta.TextoOuNulo("offerCoupon"),
                Promocao = new PromocaoEncontrada
                {
                    Produto = titulo.Trim(),
                    Loja = oferta.TextoOuNulo("storeName")?.Trim(),
                    LojaChinesa = loja != null && LojasChinesas.Contains(loja),
                    Link = $"{PromobitPagina.UrlBase}/oferta/{Uri.EscapeDataString(slug)}/",
                    Imagem = MontaUrlImagem(oferta.TextoOuNulo("offerPhoto")),
                    Preco = preco,
                    PrecoAntigo = precoAntigo,
                    Publicacao = oferta.DataOuNula("offerPublished")
                }
            };
        }

        /// <summary>
        /// O JSON traz só o nome do arquivo ("/123.png"); o CDN serve em {largura}/{arquivo}, como o próprio site faz nos cards.
        /// </summary>
        private static string MontaUrlImagem(string foto)
        {
            if (string.IsNullOrWhiteSpace(foto)) return null;

            // Não usa Uri.TryCreate aqui: no Linux "/123.png" é aceito como URI absoluta (file:///123.png).
            if (foto.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                return foto;

            return $"{UrlImagens}/{foto.TrimStart('/')}";
        }

        /// <summary>Oferta com os dados do Promobit que só servem para classificar e montar os cupons.</summary>
        private class OfertaPromobit
        {
            public PromocaoEncontrada Promocao { get; set; }
            public string Categoria { get; set; }
            public string Loja { get; set; }
            public string Cupom { get; set; }
        }
    }
}
