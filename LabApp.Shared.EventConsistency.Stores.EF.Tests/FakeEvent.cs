using LabApp.Shared.EventConsistency.Abstractions;

namespace LabApp.Shared.EventConsistency.Stores.EF.Tests
{
    public class FakeEvent : BaseIntegrationEvent
    {
        public int Data { get; set; }
    }
}