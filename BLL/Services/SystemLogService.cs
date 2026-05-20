using AutoMapper;
using BLL.DTOs;
using DAL.EF.Tables;
using DAL.Repos;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Services
{
    public class SystemLogService
    {
        SystemLogRepo repo;
        Mapper mapper;

        public SystemLogService(SystemLogRepo repo)
        {
            this.repo = repo;
            mapper = MapperConfig.GetMapper();
        }

        public bool Create(SystemLogDTO l)
        {
            var converted = mapper.Map<SystemLog>(l);
            return repo.Create(converted);
        }

        public List<SystemLogDTO> Get()
        {
            var data = repo.GetAll();
            var res = mapper.Map<List<SystemLogDTO>>(data);
            return res;
        }

        public SystemLogDTO Get(int id)
        {
            var data = repo.Get(id);
            var res = mapper.Map<SystemLogDTO>(data);
            return res;
        }

        public bool Update(SystemLogDTO l)
        {
            var converted = mapper.Map<SystemLog>(l);
            return repo.Update(converted);
        }

        public bool Delete(int id)
        {
            return repo.Delete(id);
        }



    }
}
