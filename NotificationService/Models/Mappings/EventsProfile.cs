using AutoMapper;
using LabApp.Shared.EventBus.Events;
using NotificationService.DomainEvents;

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