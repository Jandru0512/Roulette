using Masiv.Roulette.Model;
using Masiv.Roulette.Service;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace Masiv.Roulette.Data
{
    public class RouletteRepository : BaseRepository<Model.Roulette>, IRouletteRepository
    {
        protected override string TableName => Tables.Roulettes;
        public RouletteRepository(ITransactionalWrapper conection, IQueryHelper queryHelper) : base(conection, queryHelper)
        {
        }

        public async Task<List<Bet>> CloseRoulette(Model.Roulette roulette)
        {
            StringBuilder builder = new();
            builder.Append($"UPDATE {TableName} {await UpdateSingle(roulette)};");
            builder.Append($"UPDATE {Tables.Bets} SET Status = {roulette.Status} WHERE RouletteId = {roulette.Id};");
            builder.Append($"UPDATE {Tables.Bets} AS bets SET Prize = bets.Value * 5 WHERE RouletteId = {roulette.Id} AND Number = {roulette.Winner};");
            builder.Append($"UPDATE {Tables.Bets} AS bets SET Prize = bets.Value * 1.8 WHERE RouletteId = {roulette.Id} AND Color = '{(roulette.Winner % 2 == 0 ? Color.Red : Color.Black)}';");
            builder.Append($"SELECT * FROM {Tables.Bets} WHERE RouletteId = {roulette.Id};");
            DbConnection connection = await _conectionWrapper.GetConnectionAsync();
            DbCommand command = connection.CreateCommand();
            command.CommandText = builder.ToString();
            command.CommandType = CommandType.Text;
            await command.ExecuteNonQueryAsync();
            await CommitTransaction();

            using (DbDataReader reader = await command.ExecuteReaderAsync())
            {
                return _queryHelper.CreateList<Bet>(reader);
            }
        }

        public async Task<int> CreateRoulette(Model.Roulette roulette) =>
            await Insert(roulette);

        public async Task<Model.Roulette> GetRoulette(int id) =>
            await GetById(id);

        public async Task<bool> OpenRoulette(Model.Roulette roulette) =>
            await Update(roulette);

        protected override Task<string> InsertSingle(Model.Roulette roulette)
        {
            StringBuilder builder = new();
            builder.Append($"(Status, CreatedAt) ");
            builder.Append($"VALUES({roulette.Status}, '{roulette.CreatedAt:yyyy-MM-dd H:mm:ss}')");

            return Task.FromResult(builder.ToString());
        }

        protected override Task<string> UpdateSingle(Model.Roulette roulette)
        {
            StringBuilder builder = new();
            builder.Append($"SET Status = {roulette.Status}, Winner = {roulette.Winner}, UpdatedAt = '{roulette.UpdatedAt:yyyy-MM-dd H:mm:ss}' ");
            builder.Append($"WHERE id = {roulette.Id}");

            return Task.FromResult(builder.ToString());
        }
    }
}
