using System.Threading.Tasks;
using AutoMapper;
using LabApp.Shared.EventBus.Events;
using LabApp.Shared.EventBus.Events.Abstractions;
using Microsoft.Extensions.Logging;
using NotificationService.DomainEvents;
using NotificationService.Services;

namespace NotificationService.EventBus.Handlers
{
    public class NewMessageEventHandler : IntegrationEventHandlerBase<NewMessageEvent>
    {
        private readonly IMapper _mapper;
        private readonly NotifierProxy _notifierProxy;

        public NewMessageEventHandler(ILogger<NewMessageEventHandler> logger, IMapper mapper,
            NotifierProxy notifierProxy) :
            base(logger)
        {
            _mapper = mapper;
            _notifierProxy = notifierProxy;
        }

        protected override async Task HandleInternal(NewMessageEvent @event)
        {
            Logger.LogInformation("Recieved new message event {messageId}", @event.MessageId);

            var domainEvent = _mapper.Map<NewMessageEvent, NewMessage>(@event);
            await _notifierProxy.NewMessage(domainEvent);
        }
    }
}