using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiGestaoFinanceira.Models.PainelFinanceiro
{
    /// <summary>
    /// Resumo agregado de um mês (fixo x variável, ticket médio e limites), devolvido por
    /// SP_RESUMO_MES. Sempre uma única linha, mesmo sem lançamentos no mês. O maior gasto não
    /// está aqui: é sempre o primeiro item de <see cref="TopGasto"/>, então não se duplica.
    /// </summary>
    [Table("SP_RESUMO_MES")]
    public class ResumoMes
    {
        [Key]
        [Required]
        [Column("ID")]
        public int Id { get; set; }

        [Column("VALOR_FIXO", TypeName = "decimal(10,2)")]
        public decimal ValorFixo { get; set; }

        [Column("VALOR_VARIAVEL", TypeName = "decimal(10,2)")]
        public decimal ValorVariavel { get; set; }

        [Column("QUANTIDADE_FIXA")]
        public int QuantidadeFixa { get; set; }

        [Column("QUANTIDADE_VARIAVEL")]
        public int QuantidadeVariavel { get; set; }

        [Column("TICKET_MEDIO", TypeName = "decimal(10,2)")]
        public decimal TicketMedio { get; set; }

        [Column("VALOR_RECEBIDO_MES", TypeName = "decimal(10,2)")]
        public decimal ValorRecebidoMes { get; set; }

        [Column("TOTAL_CATEGORIAS_COM_LIMITE")]
        public int TotalCategoriasComLimite { get; set; }

        [Column("CATEGORIAS_ESTOURADAS")]
        public int CategoriasEstouradas { get; set; }
    }
}
