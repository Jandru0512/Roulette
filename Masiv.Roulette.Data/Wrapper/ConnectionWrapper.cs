using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Masiv.Roulette.Data
{
    public class ConnectionWrapper : IDisposable
    {
        private readonly object _lock = new();
        private readonly DbConnection _connection;
        private Task<DbConnection> _openConnection;
        private static readonly Task<DbConnection> _noConnection = Task.FromResult((DbConnection)null);

        public ConnectionWrapper(DbConnection connection)
        {
            _connection = connection;
        }

        public async Task<DbConnection> GetConnectionAsync(CancellationToken cancellationToken = default)
        {
            if (cancellationToken != default)
            {
                return await GetOpenConnectionOrOpenNewConnectionAsync(true, true, cancellationToken);
            }
            using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource())
            {
                cancellationTokenSource.CancelAfter(10000);

                return await GetOpenConnectionOrOpenNewConnectionAsync(true, true, cancellationTokenSource.Token);
            }
        }

        public DbConnection GetConnection()
        {
            Task<DbConnection> connectionTask = GetOpenConnectionOrOpenNewConnectionAsync(false, true, default);
            connectionTask.Wait();

            return connectionTask.Result;
        }

        protected Task<DbConnection> GetOpenConnectionOrOpenNewConnectionAsync(bool async, bool openIfNotOpenAlready, CancellationToken cancellationToken)
        {
            lock (_lock)
            {
                if (_openConnection != null)
                {
                    return _openConnection;
                }
                if (openIfNotOpenAlready)
                {
                    _openConnection = async
                        ? BuildConnectionAsync()
                        : BuildConnection();

                    return NotifyConnectionOpened(_openConnection);
                }

                return _noConnection;
            }

            async Task<DbConnection> BuildConnectionAsync()
            {
                await _connection.OpenAsync(cancellationToken);

                return _connection;
            }

            Task<DbConnection> BuildConnection()
            {
                _connection.Open();

                return Task.FromResult(_connection);
            }

            async Task<DbConnection> NotifyConnectionOpened(Task<DbConnection> connection)
            {
                DbConnection dbConnection = await connection;
                await OnConnectionOpened(dbConnection, async);

                return dbConnection;
            }
        }

        protected virtual Task OnConnectionOpened(DbConnection connection, bool async)
        {
            return Task.CompletedTask;
        }

        public virtual void Dispose()
        {
            _connection?.Dispose();
        }
    }
}
