using System;
using LabApp.Shared.EventConsistency.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace LabApp.Shared.EventConsistency.Stores.EF.Tests
{
    public abstract class BaseTest : IDisposable
    {
        public BaseTest()
        {
            BuildServiceProvider();
            ServiceProvider.GetRequiredService<FakeDbContext>().Database.EnsureDeleted();
        }

        protected IServiceProvider ServiceProvider;
        
        private void BuildServiceProvider()
        {
            var sc = new ServiceCollection();
            sc.AddDbContext<FakeDbContext>(x => x.UseInMemoryDatabase($"tests_database_{Guid.NewGuid().ToString()}"));
            sc.AddSingleton<IOutboxListener>(new Mock<IOutboxListener>().Object);
            ServiceProvider = sc.BuildServiceProvider();
        }

        public void Dispose()
        {
            ServiceProvider.GetRequiredService<FakeDbContext>().Database.EnsureDeleted();
        }
    }
}