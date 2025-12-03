using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiGestaoFinanceira.Models
{
    [Table("SP_GASTOS_CENTRO_CUSTO_POR_MES_ANO")]
    public class GastosCentroCusto
    {
        [Key]
        [Required]
        [Column("ID_LANC")]
        public int Id { get; set; }
                
        [Required(ErrorMessage = "O campo Valor é obrigatório")]
        [Column("VALOR", TypeName = "decimal(10,2)")]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "O campo Valor Mes Anterior é obrigatório")]
        [Column("VALOR_MES_ANTERIOR", TypeName = "decimal(10,2)")]
        public decimal ValorMesAnterior { get; set; }

        [Required(ErrorMessage = "O campo Descrição é obrigatório")]
        [Column("MES_ANO_MES_ANTERIOR")]
        public string MesAnoMesAnterior { get; set; }

        [Required(ErrorMessage = "O campo Descrição é obrigatório")]
        [Column("DESCRICAO_CENTRO_CUSTO")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "O campo Valor é obrigatório")]
        [Column("VALOR_LIMITE", TypeName = "decimal(10,2)")]
        public decimal ValorLimite { get; set; }

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
