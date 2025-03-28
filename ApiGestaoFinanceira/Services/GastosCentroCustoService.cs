﻿using ApiGestaoFinanceira.Data;
using ApiGestaoFinanceira.Data.Dto.GastosCentroCusto;
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
    public class GastosCentroCustoService
    {
        private AppDbContext _context;
        private IMapper _mapper;

        public GastosCentroCustoService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public List<ReadGastosCentroCustoDto> RecuperaGastosCentroCusto(string data)
        {
            if (string.IsNullOrEmpty(data))
                data = Convert.ToString(DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd"));

            List<GastosCentroCusto> gastosCentroCusto;
            gastosCentroCusto = _context.GastosCentroCustos.Where(cc => cc.DataHora >= DateTime.Parse(data)).Take(10).ToList();

            if (gastosCentroCusto != null)
            {
                List<ReadGastosCentroCustoDto> readDto = _mapper.Map<List<ReadGastosCentroCustoDto>>(gastosCentroCusto);
                return readDto;
            }
            return null;
        }

        public async Task<List<GastosCentroCusto>> RecuperaGastosCentroCustoMesAno(int idUsuario, string mesAno)
        {
            var parameters = new[]
            {
                new MySqlParameter("@ID_USER", idUsuario),
                new MySqlParameter("@MES_ANO", mesAno)
            };

            var gastosCentroCusto = await _context.GastosCentroCustos
                .FromSqlRaw("CALL SP_GASTOS_CENTRO_CUSTO_POR_MES_ANO(@ID_USER, @MES_ANO)", parameters)
                .ToListAsync();

            return gastosCentroCusto.AsEnumerable().ToList();
        }


    }
}
