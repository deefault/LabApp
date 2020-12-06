using AutoMapper;
using LabApp.Shared.EventBus.Events;
using NotificationService.DomainEvents;
using NotificationService.Models.Dto;

namespace NotificationService.Models.Mappings
{
    public class SignalRProfile : Profile
    {
        public SignalRProfile()
        {
            CreateMap<NewMessage, NewMessageDto>();
        }
    }
}