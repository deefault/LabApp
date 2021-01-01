using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using LabApp.Shared.Data;
using Microsoft.EntityFrameworkCore;

namespace LabApp.Server.Data
{
    public class EfUnitOfWork<TContext> : IUnitOfWork where TContext: DbContext
    {
        public readonly TContext DbContext;
        
        public EfUnitOfWork(TContext dbContext)
        {
            DbContext = dbContext;
        }

        public TransactionScope BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            var transaction = new TransactionScope(new CommittableTransaction(new TransactionOptions()
                {IsolationLevel = isolationLevel}));

            return transaction;
        }

        public void SaveChanges()
        {
            DbContext.SaveChanges();
        }

        public Task SaveChangesAsync()
        {
            return DbContext.SaveChangesAsync();
        }
        
        public Task SaveChangesAsync(CancellationToken token)
        {
            return DbContext.SaveChangesAsync(token);
        }
    }
}