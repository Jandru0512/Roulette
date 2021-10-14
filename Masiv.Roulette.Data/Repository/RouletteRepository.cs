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

        public async Task<int> CreateRoulette(Model.Roulette roulette) =>
            await Insert(roulette);

        public override Task<string> InsertSingle(Model.Roulette roulette)
        {
            StringBuilder builder = new();
            builder.Append($"(Status, CreatedAt) ");
            builder.Append($"VALUES({roulette.Status}, '{roulette.CreatedAt:yyyy-MM-dd H:mm:ss}')");

            return Task.FromResult(builder.ToString());
        }

        public override Task<string> UpdateSingle(Model.Roulette roulette)
        {
            StringBuilder builder = new();
            builder.Append($"SET Status = {roulette.Status}, UpdatedAt = '{roulette.UpdatedAt:yyyy-MM-dd H:mm:ss}' ");
            builder.Append($"WHERE id = {roulette.Id}");

            return Task.FromResult(builder.ToString());
        }
    }
}
