using AutoMapper;
using LabApp.Shared.EventBus.Events;
using NotificationService.DomainEvents;
using NotificationService.Models.Dto;

namespace NotificationService.Models.Mappings
{
    public class EventsProfile : Profile
    {
        public EventsProfile()
        {
            CreateMap<NewMessageEvent, NewMessage>();
        }
    }
}