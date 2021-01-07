namespace LabApp.Shared.EventConsistency.Stores.EF.Tests
{
    public class FakeEntity : EventEntity
    {
        public int Id { get; set; }
        public string SomeData { get; set; }
    }
}