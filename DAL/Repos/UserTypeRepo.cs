using DAL.EF;
using DAL.EF.Tables;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repos
{
    public class UserTypeRepo
    {
        CafeteriaDbContext db;
        public UserTypeRepo(CafeteriaDbContext db)
        {
            this.db = db;
        }

        public bool Create(UserType userType)
        {
            db.UserTypes.Add(userType);
            return db.SaveChanges() > 0;
        }

        public UserType Get(int id)
        {
            return db.UserTypes.Find(id);
        }

        public UserType GetByName(string name)
        {
            return db.UserTypes.FirstOrDefault(ut => ut.Name == name);
        }

        public List<UserType> GetAll()
        {
            return db.UserTypes.ToList();
        }

        public bool Update(UserType userType)
        {
            var exObj = Get(userType.Id);
            if (exObj == null)
                return false;
            db.Entry(exObj).CurrentValues.SetValues(userType);
            return db.SaveChanges() > 0;
        }

        public bool Delete(int id)
        {
            var exObj = Get(id);
            if (exObj == null)
                return false;
            db.UserTypes.Remove(exObj);
            return db.SaveChanges() > 0;
        }



    }
}
