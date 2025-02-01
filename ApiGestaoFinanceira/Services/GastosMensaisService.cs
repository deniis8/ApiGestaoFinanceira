﻿using ApiGestaoFinanceira.Data;
using ApiGestaoFinanceira.Data.Dto;
using ApiGestaoFinanceira.Data.Dto.GastosMensais;
using ApiGestaoFinanceira.Models;
using AutoMapper;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGestaoFinanceira.Services
{
    public class GastosMensaisService
    {
        private AppDbContext _context;
        private IMapper _mapper;

        public GastosMensaisService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<GastosMensais>> getGastosMensaisApartirDe(int idUsuario, string data)
        {
            var parameters = new[]
            {
                new MySqlParameter("@ID_USER", idUsuario),
                new MySqlParameter("@APARTIR_DE", data)
            };

            var gastosMensais = await _context.GastosMensais
                .FromSqlRaw("CALL SP_GASTOS_MENSAIS(@ID_USER, @APARTIR_DE)", parameters)
                .ToListAsync();

            return gastosMensais.AsEnumerable().Take(12).ToList();
        }
    }
}
