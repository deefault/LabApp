using AutoMapper;
using LabApp.Server.Data.Models;
using LabApp.Shared.Dto;

namespace LabApp.Server.Data.MapperProfiles
{
    public class SubjectProfile : Profile
    {
        public SubjectProfile()
        {
            CreateMap<Subject, SubjectDto>().ReverseMap();
        }
    }
}