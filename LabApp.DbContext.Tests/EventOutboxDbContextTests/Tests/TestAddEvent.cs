using System;
using System.Linq;
using LabApp.Server.Data;
using LabApp.Shared.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace LabApp.DbContext.Tests.EventOutboxDbContextTests.Tests
{
    public class TestAddEvent
    {
        private IServiceProvider _serviceProvider;

        public TestAddEvent()
        {
            BuildServiceProvider();
        }

        [Fact]
        public void Should_Add_Event()
        {
            var db = _serviceProvider.GetRequiredService<FakeDbContext>();
            var uw = _serviceProvider.GetRequiredService<IUnitOfWork>();
            var entity = new FakeEntity(){Id = 1, SomeData = "qwerty"};
            db.Add(entity);
            entity.AddEvent(new FakeEvent(){Data = 42});
            
            uw.SaveChanges();
            var actual = db.EventOutbox.ToList();
            
            Assert.Single(actual);
            Assert.IsType<FakeEvent>(actual.First().EventData);
            Assert.Equal(42, ((FakeEvent)actual.First().EventData).Data);
        }
        
        [Fact]
        public void Should_Add_Events()
        {
            var db = _serviceProvider.GetRequiredService<FakeDbContext>();
            var uw = _serviceProvider.GetRequiredService<IUnitOfWork>();
            var entity = new FakeEntity(){Id = 1, SomeData = "qwerty"};
            db.Add(entity);
            entity.AddEvent(new FakeEvent(){Data = 42});
            entity.AddEvent(new FakeEvent(){Data = 43});
            
            uw.SaveChanges();
            var actual = db.EventOutbox.ToList();
            
            Assert.Equal(2, actual.Count);
        }
        
        [Fact]
        public void Should_Not_Add_Events_When_Entity_Detached()
        {
            var db = _serviceProvider.GetRequiredService<FakeDbContext>();
            var uw = _serviceProvider.GetRequiredService<IUnitOfWork>();
            var entity = new FakeEntity(){Id = 1, SomeData = "qwerty"};
            //db.Add(entity);
            entity.AddEvent(new FakeEvent(){Data = 42});

            uw.SaveChanges();
            var actual = db.EventOutbox.ToList();
            
            Assert.Empty(actual);
        }

        private void BuildServiceProvider()
        {
            var sc = new ServiceCollection();
            sc.AddDbContext<FakeDbContext>(x => x.UseInMemoryDatabase("tests_database"));
            sc.AddScoped<IUnitOfWork, EfUnitOfWorkEventDecorator<FakeDbContext>>((IServiceProvider sp) =>
                new EfUnitOfWorkEventDecorator<FakeDbContext>(
                    new EfUnitOfWork<FakeDbContext>(_serviceProvider.GetRequiredService<FakeDbContext>())));
            _serviceProvider = sc.BuildServiceProvider();
        }
    }
}