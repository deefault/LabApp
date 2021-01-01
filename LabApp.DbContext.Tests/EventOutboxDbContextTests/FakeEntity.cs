using LabApp.Shared.Data;

namespace LabApp.DbContext.Tests.EventOutboxDbContextTests
{
    public class FakeEntity : EventEntity
    {
        public int Id { get; set; }
        public string SomeData { get; set; }
    }
}