using AutoMapper;
using LabApp.Server.Data.Models;
using LabApp.Shared.Dto;

namespace LabApp.Server.Data.MapperProfiles
{
    public class GroupProfile : Profile
    {
        public GroupProfile()
        {
            CreateMap<Group, GroupDto>().ReverseMap();
            
            CreateMap<Group, GroupDetailsTeacherDto>();
        }
    }
}