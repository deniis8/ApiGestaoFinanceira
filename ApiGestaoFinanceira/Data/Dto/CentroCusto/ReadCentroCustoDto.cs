using ApiGestaoFinanceira.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiGestaoFinanceira.Data.Dto
{
    public class ReadCentroCustoDto
    {
        [Key]
        [Required]
        [Column("ID_CCUSTO")]
        public int Id { get; set; }
        [Required(ErrorMessage = "O campo Descrição do Centro de Custo é obrigatório")]
        [Column("DESCRI")]
        public string DescriCCusto { get; set; }
    }
}
