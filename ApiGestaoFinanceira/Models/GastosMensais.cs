﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiGestaoFinanceira.Models
{
    [Table("VW_GASTOS_MENSAIS")]
    public class GastosMensais
    {
        [Key]
        [Required]
        [Column("ID_GASTO_MENSAL")]
        public int Id { get; set; }

        [Column("VALOR", TypeName = "decimal(10,2)")]
        public decimal Valor { get; set; }

        [Column("ANO")]
        public int Ano { get; set; }

        [Column("MES")]
        public string Mes { get; set; }

        [Column("DATA_HORA")]
        public DateTime DataHora { get; set; }

        [Column("SOBRA_MES", TypeName = "decimal(10,2)")]
        public decimal SobraMes { get; set; }

        [Column("VALOR_RECEBIDO_MES", TypeName = "decimal(10,2)")]
        public decimal ValorRecebidoMes { get; set; }

        [Column("ID_USUARIO")]
        public int IdUsuario { get; set; }
    }
}