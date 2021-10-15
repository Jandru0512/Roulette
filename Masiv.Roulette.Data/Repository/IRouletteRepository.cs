using System.Threading.Tasks;

namespace Masiv.Roulette.Data
{
    public interface IRouletteRepository
    {
        Task<int> CreateRoulette(Model.Roulette roulette);
        Task<Model.Roulette> GetRoulette(int id);
        Task<bool> OpenRoulette(Model.Roulette roulette);
    }
}