using AutoMapper;
using LabApp.Server.Data.Models;
using LabApp.Server.Data.Models.Abstractions;
using LabApp.Server.Data.Models.Attachments;
using LabApp.Server.Services.StudentServices;
using LabApp.Shared.Dto;
using LabApp.Shared.Dto.Student;

// ReSharper disable once CheckNamespace
namespace LabApp.Server.Data.MapperProfiles.StudentProfiles
{
	public class AssignmentProfile : Profile
	{
		public AssignmentProfile()
		{
			CreateMap<AssignmentWithConcrete, AssignmentDtoStudent>()
				.IncludeMembers(x => x.Assignment);
			//.ForMember(x => x.StudentAssignmentId, opt => opt.MapFrom(src => src.StudentAssignment.Id))
			//.ForAllOtherMembers(opt => opt.Ignore());
			CreateMap<AssignmentWithConcrete, AssignmentDetailsDtoStudent>()
				.IncludeBase<AssignmentWithConcrete, AssignmentDtoStudent>();

			CreateMap<StudentAssignment, StudentAssignmentDto>()
				//.ForMember(x => x.Attachments, opt => opt.Ignore())
				.ReverseMap()
				.ForMember(x => x.Body, opt => opt.MapFrom(src => src.Body))
				//.ForMember(x => x.Attachments, opt => opt.MapFrom(src => src.Attachments))
				.ForAllOtherMembers(opt => opt.Ignore());
			//.ForMember(x => x.Attachments, opt => opt.Ignore());
			//CreateMap<AttachmentDto, StudentAssignmentAttachment>()
			//	.IncludeBase<AttachmentDto, Attachment>();

			CreateMap<Assignment, AssignmentDtoStudent>();
			CreateMap<Assignment, AssignmentDetailsDtoStudent>()
				.IncludeBase<Assignment, AssignmentDtoStudent>();
		}
	}
}