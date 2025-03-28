﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGestaoFinanceira.Models
{
    public class Usuario
    {
        [Key]
        [Required]
        [Column("ID_USUARIO")]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Nome Completo é obrigatório")]
        [Column("NOME_COMPLETO")]
        public string NomeCompleto { get; set; }

        [Required(ErrorMessage = "O campo Email é obrigatório")]
        [Column("EMAIL")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo Senha é obrigatório")]
        [Column("SENHA")]
        public string Senha { get; set; }

        [Required(ErrorMessage = "O campo Data Criação é obrigatório")]
        [Column("DATA_CRIACAO")]
        public DateTime DataCriacao { get; set; }

        [Column("REFRESH_TOKEN")]
        public string RefreshToken { get; set; }

        [Column("REFRESH_TOKEN_DATA_EXPIRACAO")]
        public DateTime? RefreshTokenDataExpiracao { get; set; }

        [Column("D_E_L_E_T_")]
        public char Deletado { get; set; }
    }
}
