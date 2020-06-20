using AutoMapper;
using LabApp.Server.Data.Models;
using LabApp.Shared.Dto;

namespace LabApp.Server.Data.MapperProfiles
{
    public class UserProfileProfile : Profile
    {
        public UserProfileProfile()
        {
            CreateMap<User, ProfileDto>()
                .ReverseMap()
                .ForPath(x => x.UserId, opt => opt.Ignore());

            CreateMap<Models.Teacher, TeacherProfileDto>()
                .IncludeBase<User, ProfileDto>()
                .ReverseMap();

            CreateMap<Student, StudentProfileDto>()
                .IncludeBase<User, ProfileDto>()
                .ReverseMap();
        }
    }
}