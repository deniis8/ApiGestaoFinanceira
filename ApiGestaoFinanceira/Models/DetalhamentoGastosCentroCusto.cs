using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiGestaoFinanceira.Models
{
    [Table("VW_DETALHAMENTO_GASTOS_CENTRO_CUSTO")]
    public class DetalhamentoGastosCentroCusto
    {
        [Key]
        [Required]
        [Column("ID_LANC")]
        public int Id { get; set; }
                
        [Required(ErrorMessage = "O campo Valor é obrigatório")]
        [Column("VALOR", TypeName = "decimal(10,2)")]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "O campo Descrição é obrigatório")]
        [Column("DESCRICAO_LANCAMENTO")]
        public string DescricaoLancamento { get; set; }

        [Required(ErrorMessage = "O campo Descrição é obrigatório")]
        [Column("DESCRICAO_CENTRO_CUSTO")]
        public string DescricaoCentroCusto { get; set; }

        [Required(ErrorMessage = "O campo Data Hora é obrigatório")]
        [Column("DATA_HORA")]
        public DateTime DataHora { get; set; }

        [Required(ErrorMessage = "O campo Mês Ano é obrigatório")]
        [Column("MES_ANO")]
        public string MesAno { get; set; }

        [Required(ErrorMessage = "O campo Id Usuário é obrigatório")]
        [Column("ID_USUARIO")]
        public int IdUsuario { get; set; }

    }
}
