using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace ApiGestaoFinanceira.Data.Dto.VWLancamento
{
    public class ReadVWLancamentoDto
    {
        [Key]
        [Required]
        [Column("ID_LANC")]
        public int Id { get; set; }

        [Column("DATA_HORA")]
        public DateTime DataHora { get; set; }

        [Column("VALOR", TypeName = "decimal(10,2)")]
        public decimal Valor { get; set; }

        [Column("DESCRICAO")]
        public string Descricao { get; set; }

        [Column("ID_CCUSTO")]
        public int IdCCusto { get; set; }

        [Column("DESCRI_CC")]
        public string DescriCCusto { get; set; }

        [Column("STATUS_LANC")]
        public string Status { get; set; }

        [Column("ID_USUARIO")]
        public int IdUsuario { get; set; }
    }
}
