using AutoMapper;
using LabApp.Server.Data.Models;
using LabApp.Server.Data.Models.ManyToMany;
using LabApp.Server.Data.QueryModels;
using LabApp.Shared.Dto.Teacher;

namespace LabApp.Server.Data.MapperProfiles.Teacher
{
	public class AssignmentProfile : Profile
	{
		public AssignmentProfile()
		{
			CreateMap<Assignment, AssignmentDto>()
				.ReverseMap()
				.ForMember(x => x.Id, opt => opt.Ignore())
				.ForMember(x => x.SubjectId, opt => opt.Ignore())
				.ForMember(x => x.Subject, opt => opt.Ignore());

			CreateMap<Assignment, AssignmentDetailsDto>()
				.IncludeBase<Assignment, AssignmentDto>();


			CreateMap<GroupAssignment, AssignmentDto>()
				.ForMember(x => x.DeadLine, opt => opt.MapFrom(src => src.DeadLine))
				.ForMember(x => x.Start, opt => opt.MapFrom(src => src.Start))
				.ForMember(x => x.IsHidden, opt => opt.MapFrom(src => src.IsHidden))
				;
			CreateMap<AssignmentWithGroupInfo, AssignmentDto>()
				.IncludeMembers(x => x.Assignment, x => x.GroupAssignment);

			CreateMap<AssignmentWithGroupInfo, AssignmentDetailsDto>()
				.IncludeBase<AssignmentWithGroupInfo, AssignmentDto>();
		}
	}
}