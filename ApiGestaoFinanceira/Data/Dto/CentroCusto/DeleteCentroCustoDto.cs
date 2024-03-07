using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiGestaoFinanceira.Data.Dto.CentroCusto
{
    public class DeleteCentroCustoDto
    {
        [Column("D_E_L_E_T_")]
        public char Deletado { get; set; }
    }
}
