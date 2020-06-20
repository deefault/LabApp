using AutoMapper;
using LabApp.Server.Data.Models;
using LabApp.Server.Data.Models.Attachments;
using LabApp.Shared.Dto;

namespace LabApp.Server.Data.MapperProfiles
{
    public class LessonProfile : Profile
    {
        public LessonProfile()
        {
            CreateMap<Lesson, LessonDto>()
                .ForMember(x=>x.Attachments, opt=> opt.Ignore())
                .ReverseMap()
                .ForMember(x => x.Subject, opt => opt.Ignore())
                .ForMember(x => x.Attachments, opt => opt.Ignore());

            CreateMap<LessonAttachment, AttachmentDto>()
                .ReverseMap();
        }
    }
}