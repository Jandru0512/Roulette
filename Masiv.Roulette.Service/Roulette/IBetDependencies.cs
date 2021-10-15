using Masiv.Roulette.Common;
using System.Threading.Tasks;

namespace Masiv.Roulette.Service
{
    public interface IBetDependencies
    {
        Task<int> CreateBet(BetDto betDto);
    }
}