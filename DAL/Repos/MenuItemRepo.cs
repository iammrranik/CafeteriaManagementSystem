using DAL.EF;
using DAL.EF.Tables;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repos
{
    public class MenuItemRepo
    {
        CafeteriaDbContext db;
        public MenuItemRepo(CafeteriaDbContext db)
        {
            this.db = db;
        }

        public bool Create(MenuItem item)
        {
            db.MenuItems.Add(item);
            return db.SaveChanges() > 0;
        }

        public MenuItem Get(int id)
        {
            return db.MenuItems.Find(id);
        }

        public List<MenuItem> GetAll()
        {
            return db.MenuItems.ToList();
        }

        public List<MenuItem> GetByCategory(string category)
        {
            return db.MenuItems.Where(m => m.Category == category).ToList();
        }

        public bool Update(MenuItem item)
        {
            var exObj = Get(item.Id);
            if (exObj == null)
                return false;
            db.Entry(exObj).CurrentValues.SetValues(item);
            return db.SaveChanges() > 0;
        }

        public bool Delete(int id)
        {
            var exObj = Get(id);
            if (exObj == null)
                return false;
            db.MenuItems.Remove(exObj);
            return db.SaveChanges() > 0;
        }



    }
}
