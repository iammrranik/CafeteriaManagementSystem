using AutoMapper;
using BLL.DTOs;
using DAL.EF.Tables;
using DAL.Repos;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Services
{
    public class MenuItemService
    {
        MenuItemRepo repo;
        MealBookingRepo bookingRepo;
        Mapper mapper;

        public MenuItemService(MenuItemRepo repo, MealBookingRepo bookingRepo)
        {
            this.repo = repo;
            this.bookingRepo = bookingRepo;
            mapper = MapperConfig.GetMapper();
        }

        public bool Create(MenuItemDTO m)
        {
            var converted = mapper.Map<MenuItem>(m);
            return repo.Create(converted);
        }

        public List<MenuItemDTO> Get()
        {
            var data = repo.GetAll();
            var res = mapper.Map<List<MenuItemDTO>>(data);
            return res;
        }

        public MenuItemDTO Get(int id)
        {
            var data = repo.Get(id);
            var res = mapper.Map<MenuItemDTO>(data);
            return res;
        }

        public List<MenuItemDTO> GetRecommendedMenu(int userId)
        {
            // Get user most frequently ordered item IDs
            var userBookings = bookingRepo.GetAll().Where(b => b.UserId == userId).ToList();

            var favoriteItemIds = userBookings
                .GroupBy(b => b.MenuItemId)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .ToList();

            var allItems = repo.GetAll();

            // Sort all items placing favorite items at the top
            var recommendedList = allItems
                .OrderBy(item => favoriteItemIds.Contains(item.Id) ? favoriteItemIds.IndexOf(item.Id) : int.MaxValue)
                .ToList();

            return mapper.Map<List<MenuItemDTO>>(recommendedList);
        }

        public bool Update(MenuItemDTO m)
        {
            var converted = mapper.Map<MenuItem>(m);
            return repo.Update(converted);
        }

        public bool Delete(int id)
        {
            return repo.Delete(id);
        }



    }
}
