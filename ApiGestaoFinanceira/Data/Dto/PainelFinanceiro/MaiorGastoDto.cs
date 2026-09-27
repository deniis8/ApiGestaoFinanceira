using System;

namespace ApiGestaoFinanceira.Data.Dto.PainelFinanceiro
{
    public class MaiorGastoDto
    {
        public decimal Valor { get; set; }

        public string Descricao { get; set; }

        public DateTime DataHora { get; set; }

        public string CentroCusto { get; set; }
    }
}
