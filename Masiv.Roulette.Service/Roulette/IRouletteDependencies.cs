using Masiv.Roulette.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Masiv.Roulette.Service
{
    public interface IRouletteDependencies
    {
        Task<List<BetDto>> CloseRoulette(RouletteDto rouletteDto);
        Task<int> CreateRoulette(RouletteDto rouletteDto);
        Task<RouletteDto> GetRoulette(int id);
        Task<bool> OpenRoulette(RouletteDto rouletteDto);
    }
}