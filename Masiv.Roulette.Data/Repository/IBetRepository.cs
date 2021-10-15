using Masiv.Roulette.Model;
using System.Threading.Tasks;

namespace Masiv.Roulette.Data
{
    public interface IBetRepository
    {
        Task<int> CreateBet(Bet bet);
    }
}