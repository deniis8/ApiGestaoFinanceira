using System;
using System.Collections.Generic;

namespace ApiGestaoFinanceira.Services
{
    /// <summary>
    /// Promoção como uma fonte de dados a enxerga, antes de ser consolidada com as demais.
    /// </summary>
    public class PromocaoEncontrada
    {
        public string Produto { get; set; }
        public string Loja { get; set; }
        public bool LojaChinesa { get; set; }
        public string Link { get; set; }
        public string Imagem { get; set; }
        public decimal? Preco { get; set; }
        public decimal? PrecoAntigo { get; set; }
        public List<CupomEncontrado> Cupons { get; set; } = new List<CupomEncontrado>();
        public DateTimeOffset? Publicacao { get; set; }
    }

    public class CupomEncontrado
    {
        public const string OrigemOferta = "oferta";
        public const string OrigemLoja = "loja";

        public string Codigo { get; set; }
        public string Origem { get; set; }
        public string Desconto { get; set; }
        public string Descricao { get; set; }
        public decimal? CompraMinima { get; set; }
        public DateTimeOffset? ValidoAte { get; set; }
    }
}
