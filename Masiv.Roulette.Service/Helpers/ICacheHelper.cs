using System.Threading.Tasks;

namespace Masiv.Roulette.Service
{
    public interface ICacheHelper
    {
        Task AddCache<T>(object key, T value);
        Task<T> ResolveChace<T>(object key);
    }
}