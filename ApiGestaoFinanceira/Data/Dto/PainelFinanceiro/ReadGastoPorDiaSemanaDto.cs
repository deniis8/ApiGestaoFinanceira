using System.ComponentModel.DataAnnotations.Schema;

namespace ApiGestaoFinanceira.Data.Dto.PainelFinanceiro
{
    public class ReadGastoPorDiaSemanaDto
    {
        [Column("DIA_SEMANA_NUM")]
        public int DiaSemanaNum { get; set; }

        [Column("DIA_SEMANA")]
        public string DiaSemana { get; set; }

        [Column("VALOR_TOTAL", TypeName = "decimal(10,2)")]
        public decimal ValorTotal { get; set; }

        [Column("QUANTIDADE")]
        public int Quantidade { get; set; }
    }
}
