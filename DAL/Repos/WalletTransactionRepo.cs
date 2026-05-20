using DAL.EF;
using DAL.EF.Tables;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repos
{
    public class WalletTransactionRepo
    {
        CafeteriaDbContext db;
        public WalletTransactionRepo(CafeteriaDbContext db)
        {
            this.db = db;
        }

        public bool Create(WalletTransaction transaction)
        {
            db.WalletTransactions.Add(transaction);
            return db.SaveChanges() > 0;
        }

        public WalletTransaction Get(int id)
        {
            return db.WalletTransactions.Find(id);
        }

        public List<WalletTransaction> GetAll()
        {
            return db.WalletTransactions.ToList();
        }

        public List<WalletTransaction> GetByUserId(int userId)
        {
            return db.WalletTransactions.Where(t => t.UserId == userId).ToList();
        }

        public bool Update(WalletTransaction transaction)
        {
            var exObj = Get(transaction.Id);
            if (exObj == null)
                return false;
            db.Entry(exObj).CurrentValues.SetValues(transaction);
            return db.SaveChanges() > 0;
        }

        public bool Delete(int id)
        {
            var exObj = Get(id);
            if (exObj == null)
                return false;
            db.WalletTransactions.Remove(exObj);
            return db.SaveChanges() > 0;
        }



    }
}
