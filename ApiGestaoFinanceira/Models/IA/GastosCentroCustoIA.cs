using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiGestaoFinanceira.Models
{    
    public class GastosCentroCustoIA
    {

        [Column("ANO_MES")]
        public string AnoMes { get; set; }

        [Column("DESCRICAO_CC")]
        public string DescricaoCentroCusto { get; set; }

        [Column("VALOR", TypeName = "decimal(10,2)")]
        public decimal Valor { get; set; }

    }
}
