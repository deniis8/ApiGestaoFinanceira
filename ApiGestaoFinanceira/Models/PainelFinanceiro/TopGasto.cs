using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiGestaoFinanceira.Models.PainelFinanceiro
{
    /// <summary>
    /// Um dos cinco maiores lançamentos pagos do mês, devolvido por SP_TOP_GASTOS_MES.
    /// </summary>
    [Table("SP_TOP_GASTOS_MES")]
    public class TopGasto
    {
        [Key]
        [Required]
        [Column("ID")]
        public int Id { get; set; }

        [Column("DATA_HORA")]
        public DateTime DataHora { get; set; }

        [Column("VALOR", TypeName = "decimal(10,2)")]
        public decimal Valor { get; set; }

        [Column("DESCRICAO")]
        public string Descricao { get; set; }

        [Column("DESCRICAO_CENTRO_CUSTO")]
        public string DescricaoCentroCusto { get; set; }
    }
}
