using System.Threading.Tasks;

namespace Masiv.Roulette.Data
{
    public interface IRouletteRepository
    {
        Task<int> CreateRoulette(Model.Roulette roulette);
        Task<bool> OpenRoulette(Model.Roulette roulette);
    }
}