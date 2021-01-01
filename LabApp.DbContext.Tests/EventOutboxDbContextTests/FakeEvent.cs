using LabApp.Shared.EventBus.Events.Abstractions;

namespace LabApp.DbContext.Tests.EventOutboxDbContextTests
{
    public class FakeEvent : IntegrationEvent
    {
        public int Data { get; set; }
    }
}