using ApiGestaoFinanceira.Data.Dto.Promocoes;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGestaoFinanceira.Services
{
    public class PromocoesService
    {
        private static readonly TimeSpan TempoCache = TimeSpan.FromMinutes(30);
        // Se alguma fonte falhou, o resultado pode estar incompleto: guarda menos tempo para tentar de novo logo.
        private static readonly TimeSpan TempoCacheParcial = TimeSpan.FromMinutes(5);

        private readonly IEnumerable<IFontePromocoes> _fontes;
        private readonly IMemoryCache _cache;
        private readonly ILogger<PromocoesService> _logger;

        public PromocoesService(IEnumerable<IFontePromocoes> fontes, IMemoryCache cache, ILogger<PromocoesService> logger)
        {
            _fontes = fontes;
            _cache = cache;
            _logger = logger;
        }

        /// <summary>
        /// Consulta todas as fontes e devolve as promoções do tipo pedido, das mais recentes para as mais antigas.
        /// Lança InvalidOperationException quando todas as fontes falham.
        /// </summary>
        public async Task<List<ReadPromocaoDto>> RecuperaPromocoes(TipoPromocao tipo)
        {
            var cacheKey = $"promocoes-{tipo}";
            if (_cache.TryGetValue(cacheKey, out List<ReadPromocaoDto> emCache))
                return emCache;

            var consultas = await Task.WhenAll(_fontes.Select(f => ConsultaFonte(f, tipo)));

            var falhas = consultas.Count(c => c.Falhou);
            if (falhas == consultas.Length)
                throw new InvalidOperationException("Nenhuma fonte de promoções respondeu.");

            var descricao = TiposPromocao.Descricao(tipo);
            var promocoes = consultas
                .Where(c => !c.Falhou)
                .SelectMany(c => c.Promocoes)
                // A mesma oferta pode aparecer em mais de uma categoria que compõe o tipo.
                .GroupBy(p => p.Link)
                .Select(g => g.First())
                .OrderByDescending(p => p.Publicacao ?? DateTimeOffset.MinValue)
                .Select(p => new ReadPromocaoDto
                {
                    Produto = p.Produto,
                    Loja = p.Loja,
                    LojaChinesa = p.LojaChinesa,
                    Link = p.Link,
                    Imagem = p.Imagem,
                    Tipo = descricao,
                    Preco = p.Preco,
                    PrecoAntigo = p.PrecoAntigo,
                    Cupons = p.Cupons.Select(c => new ReadCupomDto
                    {
                        Codigo = c.Codigo,
                        Origem = c.Origem,
                        Desconto = c.Desconto,
                        Descricao = c.Descricao,
                        CompraMinima = c.CompraMinima,
                        ValidoAte = c.ValidoAte
                    }).ToList()
                })
                .ToList();

            _cache.Set(cacheKey, promocoes, falhas > 0 ? TempoCacheParcial : TempoCache);
            return promocoes;
        }

        private async Task<(List<PromocaoEncontrada> Promocoes, bool Falhou)> ConsultaFonte(IFontePromocoes fonte, TipoPromocao tipo)
        {
            try
            {
                return (await fonte.BuscaPromocoes(tipo) ?? new List<PromocaoEncontrada>(), false);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Falha ao consultar promoções de {Tipo} em {Fonte}", tipo, fonte.Nome);
                return (null, true);
            }
        }
    }
}
