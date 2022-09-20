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
        [Required]
        [Column("DATA_HORA")]
        public DateTime DataHora { get; set; }
        [Required]
        [Column("VALOR")]
        public decimal Valor { get; set; }
        [Required]
        [Column("DESCRICAO")]
        public string Descricao { get; set; }
        [Required]
        [Column("ID_CCUSTO")]
        public int IdCCusto { get; set; }
        [Required]
        [Column("DESCRI_CCUSTO")]
        public string descriCCusto { get; set; }
        [Required]
        [Column("STATUS")]
        public string Status { get; set; }
    }
}
