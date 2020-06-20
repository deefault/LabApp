using AutoMapper;
using LabApp.Shared.Dto;
using LabApp.Server.Data.Models;

namespace LabApp.Server.Data.MapperProfiles
{
    public class UserRegisterProfile : Profile
    {
        public UserRegisterProfile()
        {
            CreateMap<UserRegisterDto, UserIdentity>()
                // .ForPath(x=>x.User.Middlename, opt=>opt.MapFrom(src=>src.Middlename))
                // .ForPath(x=>x.User.Surname, opt=>opt.MapFrom(src=>src.Surname))
                // .ForPath(x=>x.User.Name, opt=>opt.MapFrom(src=>src.Name))
                // .ForPath(x=>x.User.ContactEmail, opt=>opt.MapFrom(src=>src.ContactEmail))
                // .ForPath(x=>x.User.DateBirth, opt=>opt.MapFrom(src=>src.DateBirth))
                ;

            CreateMap<UserRegisterDto, User>();

            CreateMap<TeacherRegisterDto, Models.Teacher>()
                .IncludeBase<UserRegisterDto, User>();
            
            CreateMap<StudentRegisterDto, Student>()
                .IncludeBase<UserRegisterDto, User>();
        }
    }
}