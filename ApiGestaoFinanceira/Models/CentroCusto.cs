using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ApiGestaoFinanceira.Models
{
    [Table("CCUSTO")]
    public class CentroCusto
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
