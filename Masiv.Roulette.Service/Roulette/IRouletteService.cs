using Masiv.Roulette.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Masiv.Roulette.Service
{
    public interface IRouletteService
    {
        Task<List<BetDto>> CloseRoulette(int id);
        Task<int> CreateRoulette();
        Task<bool> OpenRoulette(int id);
    }
}