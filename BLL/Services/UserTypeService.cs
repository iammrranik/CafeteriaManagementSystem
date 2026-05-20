using AutoMapper;
using BLL.DTOs;
using DAL.EF.Tables;
using DAL.Repos;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Services
{
    public class UserTypeService
    {
        UserTypeRepo repo;
        Mapper mapper;

        public UserTypeService(UserTypeRepo repo)
        {
            this.repo = repo;
            mapper = MapperConfig.GetMapper();
        }

        public bool Create(UserTypeDTO u)
        {
            var converted = mapper.Map<UserType>(u);
            return repo.Create(converted);
        }

        public List<UserTypeDTO> Get()
        {
            var data = repo.GetAll();
            var res = mapper.Map<List<UserTypeDTO>>(data);
            return res;
        }

        public UserTypeDTO Get(int id)
        {
            var data = repo.Get(id);
            var res = mapper.Map<UserTypeDTO>(data);
            return res;
        }

        public bool Update(UserTypeDTO u)
        {
            var converted = mapper.Map<UserType>(u);
            return repo.Update(converted);
        }

        public bool Delete(int id)
        {
            return repo.Delete(id);
        }



    }
}
