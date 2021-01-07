using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LabApp.Shared.EventConsistency.Abstractions;
using LabApp.Shared.EventConsistency.Stores.EF.EventOutbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace LabApp.Shared.EventConsistency.Stores.EF.Tests.EfOutboxStoreTests
{
    //TODO
    public class BasicTest : BaseTest
    {
        private readonly EfOutboxEventStore _store;
        private readonly FakeDbContext _db;
        
        public BasicTest() : base()
        {
            _db = ServiceProvider.GetService<FakeDbContext>();
            _store = new EfOutboxEventStore(_db);
        }

        [Fact]
        public async Task Should_Add()
        {
            await _store.AddAsync(new FakeEvent());

            var actual = await _db.EventOutbox.ToListAsync();

            actual.Should().HaveCount(1);
        }
        
        [Fact]
        public async Task Should_DeleteEvent()
        {
            var message = _db.EventOutbox.Add(new OutboxEventMessage(){Id = Guid.NewGuid().ToString()}).Entity;
            await _db.SaveChangesAsync();
            
            var result = await _store.TryDeleteEventAsync(message);

            result.Should().BeTrue();
            var actual = await _db.EventOutbox.ToListAsync();
            actual.Should().HaveCount(1);
            actual.First().IsProcessed.Should().BeTrue();
        }
    }
}