using System.ComponentModel.DataAnnotations.Schema;

namespace ApiGestaoFinanceira.Data.Dto.PainelFinanceiro
{
    /// <summary>Forma crua de SP_RESUMO_MES; uso interno do serviço, não é devolvida direto ao front.</summary>
    public class ReadResumoMesDto
    {
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
