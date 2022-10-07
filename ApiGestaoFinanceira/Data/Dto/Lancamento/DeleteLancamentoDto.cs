using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGestaoFinanceira.Data.Dto.Lancamento
{
    public class DeleteLancamentoDto
    {
        [Column("D_E_L_E_T_")]
        public char Deletado { get; set; }
    }
}
