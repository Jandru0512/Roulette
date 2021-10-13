using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Masiv.Roulette.Data
{
    public class TransactionalWrapper : ConnectionWrapper, ITransactionalWrapper
    {
        private readonly object _lock = new();
        private DbTransaction _openTransaction;

        public TransactionalWrapper(DbConnection connection)
            : base(connection)
        {
        }

        protected override async Task OnConnectionOpened(DbConnection connection, bool async)
        {
            await base.OnConnectionOpened(connection, async);
            lock (_lock)
            {
                if (_openTransaction != null)
                {
                    throw new InvalidOperationException("You can only have 1 connection/transaction open at a time.");
                }
                _openTransaction = connection.BeginTransaction();
            }
        }

        public async Task<DbTransaction> GetTransactionAsync(CancellationToken cancellationToken = default)
        {
            await GetOpenConnectionOrOpenNewConnectionAsync(true, true, cancellationToken);

            return _openTransaction;
        }

        public DbTransaction GetTransaction()
        {
            Task<DbConnection> connectionTask = GetOpenConnectionOrOpenNewConnectionAsync(false, true, default);
            connectionTask.Wait();

            return _openTransaction;
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            DbConnection result = await GetOpenConnectionOrOpenNewConnectionAsync(true, false, cancellationToken);
            CommitTransactionInternal(result);
        }

        public void CommitTransaction()
        {
            Task<DbConnection> connection = GetOpenConnectionOrOpenNewConnectionAsync(false, false, default);
            connection.Wait();
            CommitTransactionInternal(connection.Result);
        }

        private void CommitTransactionInternal(DbConnection connection)
        {
            if (_openTransaction == null)
            {
                return;
            }
            lock (_lock)
            {
                _openTransaction.Commit();
                _openTransaction.Dispose();
                _openTransaction = connection.BeginTransaction();
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            try
            {
                GetOpenConnectionOrOpenNewConnectionAsync(false, false, default).Wait();
            }
            catch (Exception e)
            {
                if (!IsTaskCanceledException(e))
                {
                    throw;
                }
            }
            _openTransaction?.Dispose();
        }

        private static bool IsTaskCanceledException(Exception e)
        {
            if (e == null)
            {
                return false;
            }
            if (e is TaskCanceledException)
            {
                return true;
            }
            if (IsTaskCanceledException(e.InnerException))
            {
                return true;
            }

            return false;
        }
    }
}
