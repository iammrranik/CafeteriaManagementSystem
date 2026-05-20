using AutoMapper;
using BLL.DTOs;
using DAL.EF.Tables;
using DAL.Repos;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Services
{
    public class UserService
    {
        UserRepo repo;
        SystemLogRepo logRepo;
        Mapper mapper;

        public UserService(UserRepo repo, SystemLogRepo logRepo)
        {
            this.repo = repo;
            this.logRepo = logRepo;
            mapper = MapperConfig.GetMapper();
        }

        public bool Create(UserDTO u)
        {
            // Validate unique constraints
            var existingEmail = repo.GetByEmail(u.Email);
            var existingIdCard = repo.GetByIdCardNo(u.IdCardNo);

            if (existingEmail != null || existingIdCard != null)
                return false;

            var converted = mapper.Map<User>(u);
            if (repo.Create(converted))
            {
                // Log system registration event
                var log = new SystemLog
                {
                    UserId = converted.Id,
                    Message = $"User registered with Email: {u.Email}.",
                    CreatedAt = DateTime.Now
                };
                logRepo.Create(log);
                return true;
            }
            return false;
        }

        public List<UserDTO> Get()
        {
            var data = repo.GetAll();
            return mapper.Map<List<UserDTO>>(data);
        }

        public UserDTO Get(int id)
        {
            var data = repo.Get(id);
            return mapper.Map<UserDTO>(data);
        }

        public bool Update(UserDTO u, int adminId)
        {
            var converted = mapper.Map<User>(u);
            if (repo.Update(converted))
            {
                // Log profile update event
                var log = new SystemLog
                {
                    UserId = adminId,
                    Message = $"Updated profile details for User ID: {u.Id}.",
                    CreatedAt = DateTime.Now
                };
                logRepo.Create(log);
                return true;
            }
            return false;
        }

        public bool Delete(int id, int adminId)
        {
            // First delete all system logs for this user to avoid FK constraints
            var logs = logRepo.GetByUserId(id);
            foreach (var log in logs)
            {
                logRepo.Delete(log.Id);
            }

            if (repo.Delete(id))
            {
                // Log account termination event under the performing admin's ID
                if (adminId != 0 && adminId != id)
                {
                    var log = new SystemLog
                    {
                        UserId = adminId,
                        Message = $"Deleted User ID: {id}.",
                        CreatedAt = DateTime.Now
                    };
                    logRepo.Create(log);
                }
                return true;
            }
            return false;
        }



    }
}
