using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiGestaoFinanceira.Models.PainelFinanceiro
{
    /// <summary>
    /// Total pago em um dia da semana dentro do mês, devolvido por SP_GASTOS_POR_DIA_SEMANA.
    /// Sempre sete linhas (Domingo a Sábado), mesmo em dias sem nenhum gasto.
    /// </summary>
    [Table("SP_GASTOS_POR_DIA_SEMANA")]
    public class GastoPorDiaSemana
    {
        [Key]
        [Required]
        [Column("ID")]
        public int Id { get; set; }

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
