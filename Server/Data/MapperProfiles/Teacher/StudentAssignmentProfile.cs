using AutoMapper;
using LabApp.Server.Data.Models;
using LabApp.Shared.Dto.Teacher;

// ReSharper disable once CheckNamespace
namespace LabApp.Server.Data.MapperProfiles.TeacherProfiles
{
	public class StudentAssignmentProfile : Profile
	{
		public StudentAssignmentProfile()
		{
			CreateMap<StudentAssignment, StudentAssignmentDto>()
				.ForMember(x => x.GroupName, opt => opt.MapFrom(src => src.Group.GroupName));

			CreateMap<StudentAssignment, StudentAssignmentDetailDto>()
				.IncludeBase<StudentAssignment, StudentAssignmentDto>();
		}
	}
}