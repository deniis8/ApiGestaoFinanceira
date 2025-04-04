using ApiGestaoFinanceira.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiGestaoFinanceira.Data.Dto.Lancamento
{
    public class ReadLancamentoFixoDto
    {
        [Key]
        [Required]
        [Column("ID_LANC_FIXO")]
        public int Id { get; set; }

        [Column("DIA_MES")]
        public int DiaMes { get; set; }

        [Column("VALOR", TypeName = "decimal(10,2)")]
        public decimal Valor { get; set; }

        [Column("DESCRICAO")]
        public string Descricao { get; set; }

        [Column("ID_CCUSTO")]
        public int IdCCusto { get; set; }

        [Column("STATUS_LANC")]
        public string Status { get; set; }

        [Column("ID_USUARIO")]
        public int IdUsuario { get; set; }
    }
}
