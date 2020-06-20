using AutoMapper;
using LabApp.Server.Data.Models.Abstractions;
using LabApp.Shared.Dto;

namespace LabApp.Server.Data.MapperProfiles
{
    public class Common : Profile
    {
        public Common()
        {
            CreateMap<Image, ImageDto>();
            CreateMap<Attachment, AttachmentDto>();
        }
    }
}