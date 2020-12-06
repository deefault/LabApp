using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Abstractions;
using NotificationService.DomainEvents;
using NotificationService.Hubs;
using NotificationService.Models.Dto;
using NotificationService.Models.Enums;

namespace NotificationService.Services
{
    
    public class RealTimeNotifier : INotifier
    {
        private readonly IHubContext<CommonHub, ICommonHubClient> _hub;
        private readonly IMapper _mapper;

        public NotifierType Type => NotifierType.RealTime;

        public RealTimeNotifier(IHubContext<CommonHub, ICommonHubClient> hub,
            IMapper mapper)
        {
            _hub = hub;
            _mapper = mapper;
        }

        public Task NewMessage(NewMessage @event)
        {
            var data = _mapper.Map<NewMessageDto>(@event);
            //TODO fill dto: get from database

            return _hub.Clients.Users(data.Users.Select(x => x.ToString())).NewMessage(data);
        }
    }
}