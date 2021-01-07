using System.Linq;
using LabApp.Shared.EventConsistency.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace LabApp.Shared.EventConsistency.Stores.EF.Tests.EventOutboxDbContextTests
{
    public class TestAddEvent : BaseTest
    {
        public TestAddEvent() : base()
        {
        }

        [Fact]
        public void Should_Add_Event()
        {
            var db = ServiceProvider.GetRequiredService<FakeDbContext>();
            var entity = new FakeEntity() {Id = 1, SomeData = "qwerty"};
            db.Add(entity);
            entity.AddEvent(new FakeEvent() {Data = 42});

            db.SaveChanges();
            var actual = db.EventOutbox.ToList();

            Assert.Single(actual);
            Assert.IsType<FakeEvent>(actual.First().EventData);
            Assert.Equal(42, ((FakeEvent) actual.First().EventData).Data);
        }

        [Fact]
        public void Should_Add_Events()
        {
            var db = ServiceProvider.GetRequiredService<FakeDbContext>();
            var entity = new FakeEntity() {Id = 1, SomeData = "qwerty"};
            db.Add(entity);
            entity.AddEvent(new FakeEvent() {Data = 42});
            entity.AddEvent(new FakeEvent() {Data = 43});

            db.SaveChanges();
            var actual = db.EventOutbox.ToList();

            Assert.Equal(2, actual.Count);
        }

        [Fact]
        public void Should_Not_Add_Events_When_Entity_Detached()
        {
            var db = ServiceProvider.GetRequiredService<FakeDbContext>();
            var entity = new FakeEntity() {Id = 1, SomeData = "qwerty"};
            //db.Add(entity);
            entity.AddEvent(new FakeEvent() {Data = 42});

            db.SaveChanges();
            var actual = db.EventOutbox.ToList();

            Assert.Empty(actual);
        }

        private void BuildServiceProvider()
        {
            var sc = new ServiceCollection();
            sc.AddDbContext<FakeDbContext>(x => x.UseInMemoryDatabase("tests_database"));
            sc.AddSingleton<IOutboxListener>(new Mock<IOutboxListener>().Object);
            ServiceProvider = sc.BuildServiceProvider();
        }
    }
}