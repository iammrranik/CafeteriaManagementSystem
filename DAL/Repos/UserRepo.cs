using DAL.EF;
using DAL.EF.Tables;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repos
{
    public class UserRepo
    {
        CafeteriaDbContext db;
        public UserRepo(CafeteriaDbContext db)
        {
            this.db = db;
        }

        public bool Create(User user)
        {
            db.Users.Add(user);
            return db.SaveChanges() > 0;
        }

        public User Get(int id)
        {
            return db.Users.Find(id);
        }

        public User GetByIdCardNo(string idCardNo)
        {
            return db.Users.FirstOrDefault(u => u.IdCardNo == idCardNo);
        }

        public User GetByEmail(string email)
        {
            return db.Users.FirstOrDefault(u => u.Email == email);
        }

        public List<User> GetAll()
        {
            return db.Users.ToList();
        }

        public bool Update(User user)
        {
            var exObj = Get(user.Id);
            if (exObj == null)
                return false;
            db.Entry(exObj).CurrentValues.SetValues(user);
            return db.SaveChanges() > 0;
        }

        public bool Delete(int id)
        {
            var exObj = Get(id);
            if (exObj == null)
                return false;
            db.Users.Remove(exObj);
            return db.SaveChanges() > 0;
        }

        public bool UpdateWalletBalance(int userId, double amount)
        {
            var user = Get(userId);
            if (user == null)
                return false;
            user.WalletBalance = amount;
            return db.SaveChanges() > 0;
        }



    }
}
