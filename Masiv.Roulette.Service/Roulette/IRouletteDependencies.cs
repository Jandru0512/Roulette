using Masiv.Roulette.Common;
using System.Threading.Tasks;

namespace Masiv.Roulette.Service
{
    public interface IRouletteDependencies
    {
        Task<int> CreateRoulette(RouletteDto rouletteDto);
        Task<bool> OpenRoulette(RouletteDto rouletteDto);
    }
}