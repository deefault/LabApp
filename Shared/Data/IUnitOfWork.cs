using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace LabApp.Shared.Data
{
    public interface IUnitOfWork
    {
        TransactionScope BeginTransaction(IsolationLevel isolationLevel);
        void SaveChanges();
        Task SaveChangesAsync();
        Task SaveChangesAsync(CancellationToken token);
    }
}