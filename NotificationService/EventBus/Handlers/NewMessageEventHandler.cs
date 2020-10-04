using System;
using System.Threading.Tasks;
using AutoMapper;
using LabApp.Shared.EventBus.Events.Abstractions;
using LabApp.Shared.EventBus.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NotificationService.Hubs;
using NotificationService.Models.Dto;
using NotificationService.Services;

namespace NotificationService.EventBus.Handlers
{
    public class NewMessageEventHandler : IntegrationEventHandlerBase<NewMessageEvent>
    {
        private readonly IMapper _mapper;
        private readonly IServiceProvider _sp;

        public NewMessageEventHandler(ILogger<NewMessageEventHandler> logger, IMapper mapper, IServiceProvider sp) :
            base(logger)
        {
            _mapper = mapper;
            _sp = sp;
        }

        protected override Task HandleInternal(NewMessageEvent @event)
        {
            Logger.LogInformation("Recieved new message event {messageId}", @event.MessageId);

            //_hub.Clients.Users(@event.Users.Select(x => x.ToString())).NewMessage(_mapper.Map<NewMessageDto>(@event));
            //await _hub.NewMessage(_mapper.Map<NewMessageDto>(@event));
            _sp.GetService<IRealtimeNotificationService>()?.NewMessage(_mapper.Map<NewMessageDto>(@event));

            return Task.CompletedTask;
        }
    }
}