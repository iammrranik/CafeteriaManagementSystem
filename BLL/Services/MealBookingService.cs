using AutoMapper;
using BLL.DTOs;
using DAL.EF.Tables;
using DAL.Repos;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Services
{
    public class MealBookingService
    {
        MealBookingRepo repo;
        UserRepo userRepo;
        MenuItemRepo itemRepo;
        WalletTransactionRepo txRepo;
        SystemLogRepo logRepo;
        Mapper mapper;

        public MealBookingService(
            MealBookingRepo repo,
            UserRepo userRepo,
            MenuItemRepo itemRepo,
            WalletTransactionRepo txRepo,
            SystemLogRepo logRepo)
        {
            this.repo = repo;
            this.userRepo = userRepo;
            this.itemRepo = itemRepo;
            this.txRepo = txRepo;
            this.logRepo = logRepo;
            mapper = MapperConfig.GetMapper();
        }

        public bool Create(MealBookingDTO b)
        {
            // Time lock rule - no bookings after 9.00 AM
            if (b.BookingDate.Date == DateTime.Today && DateTime.Now.Hour >= 9)
                return false;

            var item = itemRepo.Get(b.MenuItemId);
            var user = userRepo.Get(b.UserId);

            // Validate item, user, and requested quantity bounds
            if (item == null || user == null || b.OrderedQuantity <= 0)
                return false;

            // Check if cafeteria has enough food in stock
            if (item.AvailableQuantity < b.OrderedQuantity)
                return false;

            // Calculate dynamic total price based on ordered quantity
            double calculatedTotalPrice = item.Price * b.OrderedQuantity;

            // Check if user has sufficient money in wallet
            if (user.WalletBalance < calculatedTotalPrice)
                return false;

            // Map DTO to Entity and assign calculated values
            var converted = mapper.Map<MealBooking>(b);
            converted.OrderedQuantity = b.OrderedQuantity;
            converted.TotalPrice = calculatedTotalPrice;
            converted.Status = "Booked";
            converted.CreatedAt = DateTime.Now;

            if (repo.Create(converted))
            {
                // Deduct the exact ordered quantity from stock
                item.AvailableQuantity -= b.OrderedQuantity;
                itemRepo.Update(item);

                // Deduct total calculated price from user wallet
                user.WalletBalance -= calculatedTotalPrice;
                userRepo.Update(user);

                // Log transaction for purchase
                var tx = new WalletTransaction
                {
                    UserId = user.Id,
                    Amount = (decimal)-calculatedTotalPrice,
                    TransactionType = "Meal_Purchase",
                    TransactionDate = DateTime.Now
                };
                txRepo.Create(tx);

                // System Log
                var log = new SystemLog
                {
                    UserId = user.Id,
                    Message = $"Successfully booked {b.OrderedQuantity}x {item.ItemName}. Total Cost: {calculatedTotalPrice} TK.",
                    CreatedAt = DateTime.Now
                };
                logRepo.Create(log);

                return true;
            }
            return false;
        }

        public List<MealBookingDTO> Get()
        {
            var data = repo.GetAll();
            var res = mapper.Map<List<MealBookingDTO>>(data);
            return res;
        }

        public MealBookingDTO Get(int id)
        {
            var data = repo.Get(id);
            var res = mapper.Map<MealBookingDTO>(data);
            return res;
        }

        public bool CancelAndRefundEntireDay(DateTime date)
        {
            // Bulk refund workflow using dynamic booking ordered quantity
            var allBookings = repo.GetAll().Where(b => b.BookingDate.Date == date.Date && b.Status == "Booked").ToList();
            if (allBookings.Count == 0) return false;

            foreach (var booking in allBookings)
            {
                var item = itemRepo.Get(booking.MenuItemId);
                var user = userRepo.Get(booking.UserId);

                if (item != null && user != null)
                {
                    booking.Status = "Cancelled";
                    repo.Update(booking);

                    // Refund total price back using table value
                    user.WalletBalance += booking.TotalPrice;
                    userRepo.Update(user);

                    var tx = new WalletTransaction
                    {
                        UserId = user.Id,
                        Amount = (decimal)booking.TotalPrice,
                        TransactionType = "Refund",
                        TransactionDate = DateTime.Now
                    };
                    txRepo.Create(tx);

                    // System Log
                    var log = new SystemLog
                    {
                        UserId = user.Id,
                        Message = $"Meal booking for {item.ItemName} on {date.ToString("yyyy-MM-dd")} was cancelled by admin. {booking.TotalPrice} TK refunded.",
                        CreatedAt = DateTime.Now
                    };
                    logRepo.Create(log);
                }
            }
            return true;
        }

        public bool Update(MealBookingDTO b)
        {
            var converted = mapper.Map<MealBooking>(b);
            return repo.Update(converted);
        }

        public bool Delete(int id)
        {
            return repo.Delete(id);
        }
    }
}
