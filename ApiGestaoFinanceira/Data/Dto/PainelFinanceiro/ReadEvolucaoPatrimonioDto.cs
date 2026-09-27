using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiGestaoFinanceira.Data.Dto.PainelFinanceiro
{
    public class ReadEvolucaoPatrimonioDto
    {
        [Column("ANO")]
        public int Ano { get; set; }

        [Column("MES_NUM")]
        public int MesNum { get; set; }

        [Column("MES")]
        public string Mes { get; set; }

        [Column("SALDO_FINAL", TypeName = "decimal(10,2)")]
        public decimal SaldoFinal { get; set; }

        [Column("INVESTIMENTO_ACUMULADO", TypeName = "decimal(10,2)")]
        public decimal InvestimentoAcumulado { get; set; }

        [Column("DATA_REFERENCIA")]
        public DateTime DataReferencia { get; set; }
    }
}
