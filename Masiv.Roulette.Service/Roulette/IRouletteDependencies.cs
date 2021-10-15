using Masiv.Roulette.Common;
using System.Threading.Tasks;

namespace Masiv.Roulette.Service
{
    public interface IRouletteDependencies
    {
        Task<int> CreateRoulette(RouletteDto rouletteDto);
        Task<RouletteDto> GetRoulette(int id);
        Task<bool> OpenRoulette(RouletteDto rouletteDto);
    }
}