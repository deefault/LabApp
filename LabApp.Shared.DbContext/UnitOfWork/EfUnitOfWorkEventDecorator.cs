using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using LabApp.Shared.Data;
using LabApp.Shared.DbContext.EventOutbox;
using LabApp.Shared.DbContext.Models;

namespace LabApp.Shared.DbContext.UnitOfWork
{
    public class EfUnitOfWorkEventDecorator<TContext> : IUnitOfWork where TContext : Microsoft.EntityFrameworkCore.DbContext, IContextWithEventOutbox
    {
        private readonly EfUnitOfWork<TContext> _uw;

        public EfUnitOfWorkEventDecorator(EfUnitOfWork<TContext> uw)
        {
            _uw = uw;
        }

        public TransactionScope BeginTransaction(IsolationLevel isolationLevel) => _uw.BeginTransaction(isolationLevel);

        public void SaveChanges()
        {
            using (TransactionScope transaction = BeginTransaction(IsolationLevel.ReadCommitted))
            {
                _uw.SaveChanges();
                AddEvents();
                _uw.SaveChanges();

                transaction.Complete();
            }
        }

        public async Task SaveChangesAsync()
        {
            using (TransactionScope transaction = BeginTransaction(IsolationLevel.ReadCommitted))
            {
                await _uw.SaveChangesAsync();
                AddEvents();
                await _uw.SaveChangesAsync();

                transaction.Complete();
            }
        }

        public async Task SaveChangesAsync(CancellationToken token)
        {
            using (TransactionScope transaction = BeginTransaction(IsolationLevel.ReadCommitted))
            {
                await _uw.SaveChangesAsync(token);
                AddEvents();
                await _uw.SaveChangesAsync(token);

                transaction.Complete();
            }
        }

        private void AddEvents()
        {
            var eventsToSave = _uw.DbContext.ChangeTracker.Entries().Select(x => x.Entity)
                .Where(x => x is EventEntity {Events: { }}).Cast<EventEntity>().SelectMany(x => x.Events);
            _uw.DbContext.EventOutbox.AddRange(eventsToSave.Select(x => new EventMessage
            {
                Id = x.Id,
                DateTime = DateTime.UtcNow,
                EventData = x
            }));
        }
    }
}