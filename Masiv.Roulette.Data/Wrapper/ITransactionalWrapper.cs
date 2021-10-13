using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Masiv.Roulette.Data
{
    public interface ITransactionalWrapper
    {
        void CommitTransaction();
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        void Dispose();
        DbTransaction GetTransaction();
        Task<DbTransaction> GetTransactionAsync(CancellationToken cancellationToken = default);
        DbConnection GetConnection();
        Task<DbConnection> GetConnectionAsync(CancellationToken cancellationToken = default);
    }
}