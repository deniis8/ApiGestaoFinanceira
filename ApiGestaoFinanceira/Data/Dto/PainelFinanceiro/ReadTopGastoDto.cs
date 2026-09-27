using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiGestaoFinanceira.Data.Dto.PainelFinanceiro
{
    public class ReadTopGastoDto
    {
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
