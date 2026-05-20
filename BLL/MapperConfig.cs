using AutoMapper;
using BLL.DTOs;
using DAL.EF.Tables;

namespace BLL
{
    public class MapperConfig
    {
        static MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserType, UserTypeDTO>().ReverseMap();
                cfg.CreateMap<User, UserDTO>().ReverseMap();
                cfg.CreateMap<MenuItem, MenuItemDTO>().ReverseMap();
                cfg.CreateMap<MealBooking, MealBookingDTO>().ReverseMap();
                cfg.CreateMap<WalletTransaction, WalletTransactionDTO>().ReverseMap();
                cfg.CreateMap<SystemLog, SystemLogDTO>().ReverseMap();
            }
        );

        public static Mapper GetMapper()
        {
            return new Mapper(config);
        }



    }
}
