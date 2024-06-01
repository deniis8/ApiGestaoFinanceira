using ApiGestaoFinanceira.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiGestaoFinanceira.Data.Dto.GastosMensais
{
    public class ReadGastosCentroCustoDto
    {
        [Column("VALOR", TypeName = "decimal(10,2)")]
        public decimal Valor { get; set; }


        [Column("DESCRICAO_CENTRO_CUSTO")]
        public string Descricao { get; set; }


        [Column("DATA_HORA")]
        public DateTime DataHora { get; set; }


        [Column("ID_USUARIO")]
        public int IdUsuario { get; set; }
    }
}
