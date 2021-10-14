using System.Threading.Tasks;

namespace Masiv.Roulette.Service
{
    public interface IRouletteService
    {
        Task<int> CreateRoulette();
    }
}