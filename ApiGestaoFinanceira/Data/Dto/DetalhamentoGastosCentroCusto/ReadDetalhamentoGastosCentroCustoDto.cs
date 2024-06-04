using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiGestaoFinanceira.Data.Dto.DetalhamentoGastosCentroCusto
{
    public class ReadDetalhamentoGastosCentroCustoDto
    {
        [Column("VALOR", TypeName = "decimal(10,2)")]
        public decimal Valor { get; set; }

        [Column("DESCRICAO_LANCAMENTO")]
        public string DescricaoLancamento { get; set; }

        [Column("DESCRICAO_CENTRO_CUSTO")]
        public string DescricaoCentroCusto { get; set; }

        [Column("DATA_HORA")]
        public DateTime DataHora { get; set; }

        [Column("MES_ANO")]
        public string MesAno { get; set; }

        [Column("ID_USUARIO")]
        public int IdUsuario { get; set; }
    }
}
