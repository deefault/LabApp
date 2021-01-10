using FluentAssertions;
using LabApp.Shared.EventConsistency.Abstractions;
using LabApp.Shared.EventConsistency.Stores.EF.EventInbox;
using LabApp.Shared.EventConsistency.Stores.EF.EventOutbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace LabApp.Shared.EventConsistency.Stores.EF.Tests.DependencyInjectionTests
{
    public class DependencyInjectionTests
    {
        [Fact]
        public void Should_Build_ServiceProvider_With_ValidDbContext_Registered()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton(_ => new Mock<IOutboxListener>().Object);
            services.AddDbContext<FakeDbContext>(x => x.UseInMemoryDatabase(nameof(DependencyInjectionTests)));
            services.AddEventStore();

            var actual = services.BuildServiceProvider();

            actual.GetRequiredService<IInboxEventStore>().Should().BeOfType<EfInboxEventStore>();
            actual.GetRequiredService<IOutboxEventStore>().Should().BeOfType<EfOutboxEventStore>();
        }

        [Fact]
        public void Should_Build_ServiceProvider_With_No_Registered()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddEventStore();

            var actual = services.BuildServiceProvider();

            actual.GetRequiredService<IInboxEventStore>().Should().BeOfType<NullInboxEventStore>();
            actual.GetRequiredService<IOutboxEventStore>().Should().BeOfType<NullOutboxEventStore>();
        }
    }
}