using System;
using System.Collections.Generic;
using LabApp.Shared.EventBus.Events;
using LabApp.Shared.EventBus.Events.Abstractions;
using LabApp.Shared.EventBus.Interfaces;
using NotificationService.EventBus.Handlers;

namespace NotificationService.EventBus
{
    public class ServiceEventHandlers : IServiceEventHandlers
    {
        public IReadOnlyList<(Type TEvent, Type THandler)> Handlers { get; set; } = new[]
        {
            (typeof(NewMessageEvent), typeof(NewMessageEventHandler))
        };
    }
}