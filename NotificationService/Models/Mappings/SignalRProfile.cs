using AutoMapper;
using LabApp.Shared.EventBus.Events;
using NotificationService.Models.Dto;

namespace NotificationService.Models.Mappings
{
    public class SignalRProfile : Profile
    {
        public SignalRProfile()
        {
            CreateMap<NewMessageEvent, NewMessageDto>();
        }
    }
}