using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiGestaoFinanceira.Data.Dto.Usuario
{
    public class CreateUsuarioDto
    {
        [Column("NOME_COMPLETO")]
        public string NomeCompleto { get; set; }

        [Column("EMAIL")]
        public string Email { get; set; }

        [Column("SENHA")]
        public string Senha { get; set; }

        [Column("DATA_CRIACAO")]
        public DateTime DataCriacao { get; set; }

        [Column("D_E_L_E_T_")]
        public char Deletado { get; set; }

    }
}
