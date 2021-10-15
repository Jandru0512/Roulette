using System;
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
        protected abstract Task<string> InsertSingle(T entity);
        protected abstract Task<string> UpdateSingle(T entity);

        protected async Task<T> GetById(int id)
        {
            string sql = $"SELECT * FROM {TableName} Where id = @id";
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

        protected async Task<List<T>> GetList()
        {
            string sql = $"SELECT * FROM {TableName}";
            DbConnection connection = await _conectionWrapper.GetConnectionAsync();
            DbCommand command = connection.CreateCommand();
            command.CommandText = sql;
            command.CommandType = CommandType.Text;

            using (DbDataReader reader = await command.ExecuteReaderAsync())
            {
                return reader.Cast<T>().ToList();
            }
        }

        protected async Task<int> Insert(T entity)
        {
            string sql = $"INSERT INTO {TableName} {await InsertSingle(entity)}; SELECT LAST_INSERT_ID();";
            DbConnection connection = await _conectionWrapper.GetConnectionAsync();
            DbCommand command = connection.CreateCommand();
            command.CommandText = sql;
            command.CommandType = CommandType.Text;
            int id = Convert.ToInt32(await command.ExecuteScalarAsync());
            await CommitTransaction();

            return id;
        }

        protected async Task<bool> Update(T entity)
        {
            string sql = $"UPDATE {TableName} {await UpdateSingle(entity)};";
            DbConnection connection = await _conectionWrapper.GetConnectionAsync();
            DbCommand command = connection.CreateCommand();
            command.CommandText = sql;
            command.CommandType = CommandType.Text;
            await command.ExecuteScalarAsync();
            await CommitTransaction();

            return true;
        }

        protected async Task<int> Delete(int id)
        {
            string sql = $"DELETE FROM {TableName} WHERE id = @id";
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
            await CommitTransaction();

            return id;
        }

        protected async Task CommitTransaction()
        {
            await _conectionWrapper.CommitTransactionAsync();
        }
    }
}
