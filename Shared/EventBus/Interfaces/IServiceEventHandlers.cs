using System;
using System.Collections.Generic;
using LabApp.Shared.EventBus.Events.Abstractions;

namespace LabApp.Shared.EventBus.Interfaces
{
    public interface IServiceEventHandlers
    {
        public IReadOnlyList<(Type TEvent, Type THandler)> Handlers { get; set; }
    }
}