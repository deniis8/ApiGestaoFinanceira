using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiGestaoFinanceira.Data.Dto.Saldos
{
    public class ReadSaldosInvestimentos
    {
        [Key]
        [Column("ID_SALDO")]
        public int Id { get; set; }

        [Column("SALDO", TypeName = "decimal(10,2)")]
        public decimal Saldo { get; set; }

        [Column("INVESTIMENTO_FIXO", TypeName = "decimal(10,2)")]
        public decimal InvestimentoFixo { get; set; }

        [Column("INVESTIMENTO_VARIAVEL", TypeName = "decimal(10,2)")]
        public decimal InvestimentoVariavel { get; set; }

        [Column("DATA_CRIACAO")]
        public DateTime DataHora { get; set; }

        [Column("ID_USUARIO")]
        public int IdUsuario { get; set; }


    }
}
