using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiGestaoFinanceira.Models
{
    public class Lancamento
    {
        [Key]
        [Required]
        [Column("ID_LANC")]
        public int Id { get; set; }
        [Required(ErrorMessage = "O campo Data Hora é obrigatório")]
        [Column("DATA_HORA")]
        public DateTime DataHora { get; set; }
        [Required(ErrorMessage = "O campo Valor é obrigatório")]
        [Column("VALOR")]
        public decimal Valor { get; set; }
        [Required(ErrorMessage = "O campo Descrição é obrigatório")]
        [Column("DESCRICAO")]
        public string Descricao { get; set; }
        [Required(ErrorMessage = "O campo ID do Centro de Custo é obrigatório")]
        [Column("ID_CCUSTO")]
        public int IdCCusto { get; set; }
        [Required(ErrorMessage = "O campo Status é obrigatório")]
        [Column("STATUS")]
        public string Status { get; set; }
    }
}
