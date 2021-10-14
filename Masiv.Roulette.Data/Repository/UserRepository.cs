using Masiv.Roulette.Model;
using Microsoft.AspNetCore.Identity;
using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace Masiv.Roulette.Data
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly UserManager<User> _userManager;
        protected override string TableName => Tables.Users;

        public UserRepository(ITransactionalWrapper conection, IQueryHelper queryHelper, UserManager<User> userManager) : base(conection, queryHelper)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> AddUser(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<User> GetUser(string username)
        {
            string sql = $"SELECT Id, FirstName, LastName, UserName, Email, PhoneNumber, Credit FROM {TableName} WHERE username = @username";
            DbConnection connection = await _conectionWrapper.GetConnectionAsync();
            DbCommand command = connection.CreateCommand();
            command.CommandText = sql;
            command.CommandType = CommandType.Text;
            DbParameter parameter = command.CreateParameter();
            parameter.ParameterName = "@username";
            parameter.DbType = DbType.String;
            parameter.Value = username;
            command.Parameters.Add(parameter);

            using (DbDataReader reader = await command.ExecuteReaderAsync())
            {
                return _queryHelper.Create<User>(reader);
            }
        }

        public override Task<string> InsertSingle(User entity)
        {
            throw new NotImplementedException();
        }

        public override Task<string> UpdateSingle(User entity)
        {
            throw new NotImplementedException();
        }
    }
}
