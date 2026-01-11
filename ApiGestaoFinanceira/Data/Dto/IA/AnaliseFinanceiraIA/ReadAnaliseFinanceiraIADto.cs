using System.Collections;

namespace ApiGestaoFinanceira.Data.Dto.IA.GastosCentroCustoIA
{
    public class ReadAnaliseFinanceiraIADto
    {
        public IEnumerable LancamentosRecebidos { get; set; }
        public IEnumerable GastosPorCentroDeCusto { get; set; }
        public IEnumerable Investimentos { get; set; }
        public object Saldos { get; set; }
    }
}