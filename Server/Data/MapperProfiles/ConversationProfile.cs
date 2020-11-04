using System.Linq;
using AutoMapper;
using LabApp.Server.Data.Models;
using LabApp.Server.Data.Models.ManyToMany;
using LabApp.Server.Data.QueryModels;
using LabApp.Shared.Dto;

namespace LabApp.Server.Data.MapperProfiles
{
	public class ConversationProfile : Profile
	{
		public ConversationProfile()
		{
			CreateMap<UserConversation, UserListDto>()
				.ForMember(x => x.Name, opt => opt.MapFrom(src => src.User.Name))
				.ForMember(x => x.Surname, opt => opt.MapFrom(src => src.User.Surname))
				.ForMember(x => x.Middlename, opt => opt.MapFrom(src => src.User.Middlename))
				.ForMember(x => x.Thumbnail, opt => opt.MapFrom(src => src.User.Thumbnail))
				.ForMember(x => x.PhotoId, opt => opt.MapFrom(src => src.User.PhotoId))
				.ForMember(x => x.UserType, opt => opt.MapFrom(src => src.User.UserType))
				.ForMember(x => x.UserId, opt => opt.MapFrom(src => src.UserId));
			CreateMap<Conversation, ConversationDto>()
				.ForMember(x => x.Users, opt => opt.MapFrom(src => src.Users));
			CreateMap<Message, MessageDto>();

			CreateMap<ConversationWithLastMessage, ConversationWithLastMessageDto>();
		}
	}
}