using ApiGestaoFinanceira.Data;
using ApiGestaoFinanceira.Data.Dto.IA.GastosCentroCustoIA;
using AutoMapper;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace ApiGestaoFinanceira.Services
{
    public class ConfiguracoesIAService
    {
        private AppDbContext _context;
        private IMapper _mapper;

        public ConfiguracoesIAService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public ReadConfiguracoesIADto AdicionaConfiguracoesIA(CreateConfiguracoesIADto configuracoesIADto)
        {
            ConfiguracoesIA configuracoesIA = _mapper.Map<ConfiguracoesIA>(configuracoesIADto);
            _context.ConfiguracoesIA.Add(configuracoesIA);
            _context.SaveChanges();
            return _mapper.Map<ReadConfiguracoesIADto>(configuracoesIA);
        }

        public ReadConfiguracoesIADto RecuperaConfiguracoesPorId(int id)
        {
            ConfiguracoesIA configuracoesIA = _context.ConfiguracoesIA.FirstOrDefault(ia => ia.Id == id);
            if (configuracoesIA != null)
            {
                ReadConfiguracoesIADto configuracoesIADto = _mapper.Map<ReadConfiguracoesIADto>(configuracoesIA);

                return configuracoesIADto;
            }
            return null;
        }

        public ReadConfiguracoesIADto RecuperaConfiguracoesIAPorIdUsuario(int idUsuario)
        {
            ConfiguracoesIA configuracoesIA;
            configuracoesIA = _context.ConfiguracoesIA.Where(ia => ia.Deletado.ToString() != "*" && ia.IdUsuario == idUsuario).FirstOrDefault();

            if (configuracoesIA != null)
            {
                ReadConfiguracoesIADto readDto = _mapper.Map<ReadConfiguracoesIADto>(configuracoesIA);
                return readDto;
            }
            return null;
        }

        public Result AtualizaConfiguracoesIA(int id, UpdateConfiguracoesIADto configuracoesIADto)
        {
            ConfiguracoesIA configuracoesIA = _context.ConfiguracoesIA.FirstOrDefault(ia => ia.Id == id);
            if (configuracoesIA == null)
            {
                return Result.Fail("Centro de Custo não encontrado");
            }
            _mapper.Map(configuracoesIADto, configuracoesIA);
            _context.SaveChanges();
            return Result.Ok();
        }
    }
}