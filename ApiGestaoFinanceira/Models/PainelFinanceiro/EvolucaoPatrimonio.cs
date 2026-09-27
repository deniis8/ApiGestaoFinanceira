using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiGestaoFinanceira.Models.PainelFinanceiro
{
    /// <summary>
    /// Saldo de fechamento de cada mês (o último valor da tabela SALDOS naquele mês) mais
    /// quanto já foi acumulado em Investimento Fixo/Variável até o fim daquele mês, devolvido
    /// por SP_EVOLUCAO_PATRIMONIO. SaldoFinal é só o líquido (todo Pago já é descontado dali,
    /// inclusive os aportes em investimento); some os dois para o patrimônio total.
    /// </summary>
    [Table("SP_EVOLUCAO_PATRIMONIO")]
    public class EvolucaoPatrimonio
    {
        [Key]
        [Required]
        [Column("ID")]
        public int Id { get; set; }

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
