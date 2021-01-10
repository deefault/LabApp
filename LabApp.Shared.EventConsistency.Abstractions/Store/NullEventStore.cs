using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LabApp.Shared.EventConsistency.Abstractions
{
    public class NullOutboxEventStore : IOutboxEventStore
    {
        private class OutboxEventStoreNotRegisteredException : InvalidOperationException
        {
            public OutboxEventStoreNotRegisteredException() : base(
                $"Tried to use {nameof(NullOutboxEventStore)} for service {nameof(IOutboxEventStore)}")
            {
            }
        }

        public Task AddAsync(BaseIntegrationEvent @event)
        {
            throw new OutboxEventStoreNotRegisteredException();
        }

        public Task<EventMessage> GetEventAsync(string id)
        {
            throw new OutboxEventStoreNotRegisteredException();
        }

        public Task<IEnumerable<EventMessage>> GetEventsAsync(IEnumerable<string> ids)
        {
            throw new OutboxEventStoreNotRegisteredException();
        }

        public Task<bool> TryDeleteEventAsync(EventMessage message)
        {
            throw new OutboxEventStoreNotRegisteredException();
        }

        public Task DeleteEventsAsync(IEnumerable<EventMessage> messages)
        {
            throw new OutboxEventStoreNotRegisteredException();
        }

        public Task<IEnumerable<EventMessage>> GetUnpublishedAsync(TimeSpan maxTime)
        {
            throw new OutboxEventStoreNotRegisteredException();
        }
    }

    public class NullInboxEventStore : IInboxEventStore
    {
        private class InboxEventStoreNotRegisteredException : InvalidOperationException
        {
            public InboxEventStoreNotRegisteredException() : base(
                $"Tried to use {nameof(NullInboxEventStore)} for service {nameof(IInboxEventStore)}")
            {
            }
        }

        public Task AddAsync(BaseIntegrationEvent @event)
        {
            throw new InboxEventStoreNotRegisteredException();
        }

        public Task<EventMessage> GetEventAsync(string id)
        {
            throw new InboxEventStoreNotRegisteredException();
        }

        public Task<IEnumerable<EventMessage>> GetEventsAsync(IEnumerable<string> ids)
        {
            throw new InboxEventStoreNotRegisteredException();
        }

        public Task<bool> TryDeleteEventAsync(EventMessage message)
        {
            throw new InboxEventStoreNotRegisteredException();
        }

        public Task DeleteEventsAsync(IEnumerable<EventMessage> messages)
        {
            throw new InboxEventStoreNotRegisteredException();
        }

        public Task<IEnumerable<EventMessage>> GetUnpublishedAsync(TimeSpan maxTime)
        {
            throw new InboxEventStoreNotRegisteredException();
        }

        public Task IncrementFailedAsync(InboxEventMessage eventMessage)
        {
            throw new InboxEventStoreNotRegisteredException();
        }
    }
}