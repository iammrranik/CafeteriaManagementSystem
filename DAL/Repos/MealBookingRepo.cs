using DAL.EF;
using DAL.EF.Tables;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repos
{
    public class MealBookingRepo
    {
        CafeteriaDbContext db;
        public MealBookingRepo(CafeteriaDbContext db)
        {
            this.db = db;
        }

        public bool Create(MealBooking booking)
        {
            db.MealBookings.Add(booking);
            return db.SaveChanges() > 0;
        }

        public MealBooking Get(int id)
        {
            return db.MealBookings.Find(id);
        }

        public List<MealBooking> GetAll()
        {
            return db.MealBookings.ToList();
        }

        // Fetch all bookings for a specific user
        public List<MealBooking> GetByUserId(int userId)
        {
            return db.MealBookings.Where(b => b.UserId == userId).ToList();
        }

        public bool Update(MealBooking booking)
        {
            var exObj = Get(booking.Id);
            if (exObj == null)
                return false;
            db.Entry(exObj).CurrentValues.SetValues(booking);
            return db.SaveChanges() > 0;
        }

        public bool Delete(int id)
        {
            var exObj = Get(id);
            if (exObj == null)
                return false;
            db.MealBookings.Remove(exObj);
            return db.SaveChanges() > 0;
        }



    }
}
