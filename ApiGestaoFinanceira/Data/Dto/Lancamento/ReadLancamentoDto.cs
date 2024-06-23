using ApiGestaoFinanceira.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiGestaoFinanceira.Data.Dto.Lancamento
{
    public class ReadLancamentoDto
    {
        [Key]
        [Required]
        [Column("ID_LANC")]
        public int Id { get; set; }

        [Column("DATA_HORA")]
        public DateTime DataHora { get; set; }

        [Column("VALOR", TypeName = "decimal(10,2)")]
        public decimal Valor { get; set; }

        [Column("DESCRICAO")]
        public string Descricao { get; set; }

        [Column("ID_CCUSTO")]
        public int IdCCusto { get; set; }

        [Column("DESCRI_CC")]
        public string DescriCCusto { get; set; }

        [Column("STATUS_LANC")]
        public string Status { get; set; }

        [Column("ID_USUARIO")]
        public int IdUsuario { get; set; }
    }
}
