using ApiGestaoFinanceira.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiGestaoFinanceira.Data.Dto.Lancamento
{
    public class ReadLancamentosRecebidosIADto
    {
        [Column("DATA_HORA")]
        public DateTime DataHora { get; set; }

        [Column("VALOR", TypeName = "decimal(10,2)")]
        public decimal Valor { get; set; }

        [Column("DESCRICAO")]
        public string Descricao { get; set; }

        [Column("STATUS_LANC")]
        public string Status { get; set; }
    }
}
