using AutoMapper;
using BLL.DTOs;
using DAL.EF.Tables;
using DAL.Repos;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Services
{
    public class WalletTransactionService
    {
        WalletTransactionRepo repo;
        UserRepo userRepo;
        Mapper mapper;

        public WalletTransactionService(WalletTransactionRepo repo, UserRepo userRepo)
        {
            this.repo = repo;
            this.userRepo = userRepo;
            mapper = MapperConfig.GetMapper();
        }

        public bool Create(WalletTransactionDTO t)
        {
            var user = userRepo.Get(t.UserId);
            if (user == null || t.Amount <= 0) return false;

            var converted = mapper.Map<WalletTransaction>(t);
            converted.TransactionDate = DateTime.Now;

            if (repo.Create(converted))
            {
                // Minimal Comment: Top-up the main user wallet balance on successful deposit
                user.WalletBalance += (double)t.Amount;
                return userRepo.Update(user);
            }
            return false;
        }

        public List<WalletTransactionDTO> Get()
        {
            var data = repo.GetAll();
            var res = mapper.Map<List<WalletTransactionDTO>>(data);
            return res;
        }

        public WalletTransactionDTO Get(int id)
        {
            var data = repo.Get(id);
            var res = mapper.Map<WalletTransactionDTO>(data);
            return res;
        }

        public bool Update(WalletTransactionDTO t)
        {
            var converted = mapper.Map<WalletTransaction>(t);
            return repo.Update(converted);
        }

        public bool Delete(int id)
        {
            return repo.Delete(id);
        }



    }
}
