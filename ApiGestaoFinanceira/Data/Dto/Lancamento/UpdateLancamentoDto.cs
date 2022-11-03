using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiGestaoFinanceira.Data.Dto.Lancamento
{
    public class UpdateLancamentoDto
    {
        //[Required(ErrorMessage = "O campo Data Hora é obrigatório")]
        [Column("DATA_HORA")]
        public DateTime DataHora { get; set; }
        //[Required(ErrorMessage = "O campo Valor é obrigatório")]
        [Column("VALOR", TypeName = "decimal(10,2)")]
        public decimal Valor { get; set; }
        //[Required(ErrorMessage = "O campo Descrição é obrigatório")]
        [Column("DESCRICAO")]
        public string Descricao { get; set; }
        [Column("ID_CCUSTO")]
        //[Required(ErrorMessage = "O campo ID do Centro de Custo é obrigatório")]
        public int IdCCusto { get; set; }

        //[Required(ErrorMessage = "O campo Status é obrigatório")]
        [Column("STATUS_LANC")]
        public string Status { get; set; }

        //[Required(ErrorMessage = "O campo Data Criação é obrigatório")]
        [Column("DATA_CRIACAO")]
        public DateTime DataCriacao { get; set; }

        //[Required(ErrorMessage = "O campo Id Usuário é obrigatório")]
        [Column("ID_USUARIO")]
        public int IdUsuario { get; set; }

        [Column("D_E_L_E_T_")]
        public char Deletado { get; set; }
    }
}
