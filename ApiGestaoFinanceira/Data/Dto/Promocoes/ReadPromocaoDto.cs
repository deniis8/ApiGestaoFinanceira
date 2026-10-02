using System;
using System.Collections.Generic;

namespace ApiGestaoFinanceira.Data.Dto.Promocoes
{
    public class ReadPromocaoDto
    {
        public string Produto { get; set; }
        public string Loja { get; set; }
        public bool LojaChinesa { get; set; }
        public string Link { get; set; }
        public string Imagem { get; set; }
        public string Tipo { get; set; }
        public decimal? Preco { get; set; }
        public decimal? PrecoAntigo { get; set; }
        public List<ReadCupomDto> Cupons { get; set; }
    }

    public class ReadCupomDto
    {
        public string Codigo { get; set; }
        /// <summary>"oferta": indicado para este produto. "loja": cupom geral da loja que cabe no preço do produto.</summary>
        public string Origem { get; set; }
        public string Desconto { get; set; }
        public string Descricao { get; set; }
        public decimal? CompraMinima { get; set; }
        public DateTimeOffset? ValidoAte { get; set; }
    }
}
