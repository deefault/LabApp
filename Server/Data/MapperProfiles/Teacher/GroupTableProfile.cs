using AutoMapper;
using LabApp.Server.Controllers.TeacherControllers;
using LabApp.Shared.Dto.Teacher;

namespace LabApp.Server.Data.MapperProfiles.Teacher
{
	public class GroupTableProfile : Profile
	{
		public GroupTableProfile()
		{
			CreateMap<GroupController.GroupTableData, GroupTableEntryDto>()
				.ForMember(x => x.Student, opt => opt.MapFrom(src => src.Student))
				.ForMember(x => x.StudentAssignments, opt => opt.MapFrom(src => src.StudentAssignments));
		}
	}
}