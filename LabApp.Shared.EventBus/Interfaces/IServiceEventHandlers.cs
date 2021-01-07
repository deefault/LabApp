using System;
using System.Collections.Generic;

namespace LabApp.Shared.EventBus.Interfaces
{
    public interface IServiceEventHandlers
    {
        public IReadOnlyList<(Type TEvent, Type THandler)> Handlers { get; set; }
    }
}