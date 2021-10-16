using System.Collections.Generic;
using System.Threading.Tasks;

namespace Masiv.Roulette.Service
{
    public interface ICacheHelper
    {
        Task AddCache<T>(T value, string key, string dependency = "root");
        Task AddListCache<T>(List<T> value, string key = "root");
        Task<T> ResolveCache<T>(string key);
        Task<List<T>> ResolveListCache<T>(string key = "root");
    }
}