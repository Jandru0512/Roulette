using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Masiv.Roulette.Data
{
    public abstract class BaseRepository<T> where T : class
    {
        protected readonly IQueryHelper _queryHelper;
        protected readonly ITransactionalWrapper _conectionWrapper;

        public BaseRepository(ITransactionalWrapper conection, IQueryHelper queryHelper)
        {
            _conectionWrapper = conection;
            _queryHelper = queryHelper;
        }

        protected abstract string TableName { get; }
        public abstract Task<T> InsertSingle(T obj);
        public abstract Task<T> UpdateSingle(T obj);

        public async Task<T> GetById(int id)
        {
            string sql = $"delete from {TableName} Where id = @id";
            DbConnection connection = await _conectionWrapper.GetConnectionAsync();
            DbCommand command = connection.CreateCommand();
            command.CommandText = sql;
            command.CommandType = CommandType.Text;
            DbParameter parameter = command.CreateParameter();
            parameter.ParameterName = "@id";
            parameter.DbType = DbType.Int32;
            parameter.Value = id;
            command.Parameters.Add(parameter);

            using (DbDataReader reader = await command.ExecuteReaderAsync())
            {
                return _queryHelper.Create<T>(reader);
            }
        }

        public async Task<List<T>> GetList()
        {
            string sql = $"select * from {TableName}";
            DbConnection connection = await _conectionWrapper.GetConnectionAsync();
            DbCommand command = connection.CreateCommand();
            command.CommandText = sql;
            command.CommandType = CommandType.Text;

            using (DbDataReader reader = await command.ExecuteReaderAsync())
            {
                return reader.Cast<T>().ToList();
            }
        }


        public async Task<List<T>> InsertList(List<T> obj)
        {
            return (await Task.WhenAll(obj.Select(a => InsertSingle(a)))).ToList();
        }

        public async Task<List<T>> UpdateList(List<T> obj)
        {
            return (await Task.WhenAll(obj.Select(a => UpdateSingle(a)))).ToList();
        }

        public async Task<int> Delete(int id)
        {
            string sql = $"delete from {TableName} Where id = @id";
            DbConnection connection = await _conectionWrapper.GetConnectionAsync();
            DbCommand command = connection.CreateCommand();
            command.CommandText = sql;
            command.CommandType = CommandType.Text;
            DbParameter parameter = command.CreateParameter();
            parameter.ParameterName = "@id";
            parameter.DbType = DbType.Int32;
            parameter.Value = id;
            command.Parameters.Add(parameter);
            await command.ExecuteNonQueryAsync();

            return id;
        }

        public async Task CommitTransaction()
        {
            await _conectionWrapper.CommitTransactionAsync();
        }
    }
}
