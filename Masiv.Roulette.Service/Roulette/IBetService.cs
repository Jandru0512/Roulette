using Masiv.Roulette.Common;
using System.Threading.Tasks;

namespace Masiv.Roulette.Service
{
    public interface IBetService
    {
        Task<int> CreateBet(BetDto bet, string username);
    }
}