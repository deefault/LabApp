using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LabApp.Shared.EventBus.Events;
using LabApp.Shared.EventBus.Events.Abstractions;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NotificationService.Hubs;
using NotificationService.Models.Dto;

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

        protected override async Task HandleInternal(NewMessageEvent @event)
        {
            Logger.LogInformation("Recieved new message event {messageId}", @event.MessageId);

            await _sp.GetRequiredService<IHubContext<CommonHub, ICommonHubClient>>().Clients
                .Users(@event.Users.Select(x => x.ToString())).NewMessage(_mapper.Map<NewMessageDto>(@event));
        }
    }
}