using AutoMapper;
using LabApp.Server.Data.Models;
using LabApp.Shared.Dto.Student;

// ReSharper disable once CheckNamespace
namespace LabApp.Server.Data.MapperProfiles.StudentProfiles
{
    public class GroupSubjectProfile : Profile
    {
        public GroupSubjectProfile()
        {
            CreateMap<Group, GroupSubjectDto>();
        }
    }
}