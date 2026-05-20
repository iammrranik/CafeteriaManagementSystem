using DAL.EF;
using DAL.EF.Tables;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repos
{
    public class SystemLogRepo
    {
        CafeteriaDbContext db;
        public SystemLogRepo(CafeteriaDbContext db)
        {
            this.db = db;
        }

        public bool Create(SystemLog log)
        {
            db.SystemLogs.Add(log);
            return db.SaveChanges() > 0;
        }

        public SystemLog Get(int id)
        {
            return db.SystemLogs.Find(id);
        }

        public List<SystemLog> GetAll()
        {
            return db.SystemLogs.ToList();
        }

        public List<SystemLog> GetByUserId(int userId)
        {
            return db.SystemLogs.Where(l => l.UserId == userId).ToList();
        }

        public bool Update(SystemLog log)
        {
            var exObj = Get(log.Id);
            if (exObj == null)
                return false;
            db.Entry(exObj).CurrentValues.SetValues(log);
            return db.SaveChanges() > 0;
        }

        public bool Delete(int id)
        {
            var exObj = Get(id);
            if (exObj == null)
                return false;
            db.SystemLogs.Remove(exObj);
            return db.SaveChanges() > 0;
        }



    }
}
