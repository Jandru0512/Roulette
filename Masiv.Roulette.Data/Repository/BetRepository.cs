using Masiv.Roulette.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masiv.Roulette.Data
{
    public class BetRepository : BaseRepository<Bet>, IBetRepository
    {
        protected override string TableName => Tables.Bets;
        public BetRepository(ITransactionalWrapper conection, IQueryHelper queryHelper) : base(conection, queryHelper)
        {
        }

        public async Task<int> CreateBet(Bet bet)
        {
            StringBuilder builder = new();
            builder.Append($"INSERT INTO {TableName} {await InsertSingle(bet)}; SELECT LAST_INSERT_ID();");
            builder.Append($"UPDATE {Tables.Users} SET Credit = {bet.User.Credit} WHERE id = '{bet.UserId}';");
            DbConnection connection = await _conectionWrapper.GetConnectionAsync();
            DbCommand command = connection.CreateCommand();
            command.CommandText = builder.ToString();
            command.CommandType = CommandType.Text;
            int id = Convert.ToInt32(await command.ExecuteScalarAsync());
            await CommitTransaction();

            return id;
        }

        protected override Task<string> InsertSingle(Bet bet)
        {
            StringBuilder builder = new();
            builder.Append($"(Color, Number, Prize, Status, Value, CreatedAt, RouletteId, UserId) ");
            builder.Append($"VALUES('{bet.Color}', {bet.Number}, {bet.Prize}, {bet.Status}, {bet.Value}, '{bet.CreatedAt:yyyy-MM-dd H:mm:ss}', {bet.RouletteId}, '{bet.UserId}')");

            return Task.FromResult(builder.ToString());
        }

        protected override Task<string> UpdateSingle(Bet entity)
        {
            throw new NotImplementedException();
        }
    }
}
