using System.Collections.Generic;

namespace ApiGestaoFinanceira.Data.Dto.PainelFinanceiro
{
    /// <summary>Payload único devolvido pelo painel de gráficos para o mês selecionado.</summary>
    public class ReadPainelMesDto
    {
        public string MesAno { get; set; }

        public decimal ValorFixo { get; set; }
        public decimal ValorVariavel { get; set; }
        public int QuantidadeFixa { get; set; }
        public int QuantidadeVariavel { get; set; }
        public decimal TicketMedio { get; set; }

        public decimal ValorRecebidoMes { get; set; }
        public decimal SobraMes { get; set; }

        /// <summary>Nulo quando o mês não teve nenhum lançamento pago.</summary>
        public MaiorGastoDto MaiorGasto { get; set; }

        public int TotalCategoriasComLimite { get; set; }
        public int CategoriasEstouradas { get; set; }

        public SaudeFinanceiraDto Saude { get; set; }

        public List<ReadTopGastoDto> TopGastos { get; set; }

        public List<ReadGastoPorDiaSemanaDto> GastosPorDiaSemana { get; set; }
    }
}
