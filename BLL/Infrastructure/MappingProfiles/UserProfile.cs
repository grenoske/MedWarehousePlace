using AutoMapper;
using BLL.DTO;
using DAL.Entities;

namespace BLL.Infrastructure.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();
        }
    }
}
