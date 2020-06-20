using AutoMapper;
using LabApp.Server.Data.Models;
using LabApp.Shared.Dto;

namespace LabApp.Server.Data.MapperProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserListDto>();
        }
    }
}