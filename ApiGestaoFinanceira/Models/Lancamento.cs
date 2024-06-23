using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiGestaoFinanceira.Models
{
    [Table("VW_LANCAMENTOS")]
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
        [Column("VALOR", TypeName = "decimal(10,2)")]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "O campo Descrição é obrigatório")]
        [Column("DESCRICAO")]
        public string Descricao { get; set; }

        [Column("ID_CCUSTO")]
        [Required(ErrorMessage = "O campo ID do Centro de Custo é obrigatório")]
        public int IdCCusto { get; set; }

        [Required(ErrorMessage = "O campo Descrição Centro de Custo é obrigatório")]
        [Column("DESCRI_CC")]
        public string DescriCCusto { get; set; }

        [Required(ErrorMessage = "O campo Status é obrigatório")]
        [Column("STATUS_LANC")]
        public string Status { get; set; }

        [Required(ErrorMessage = "O campo Id Usuário é obrigatório")]
        [Column("ID_USUARIO")]
        public int IdUsuario { get; set; }

    }
}
