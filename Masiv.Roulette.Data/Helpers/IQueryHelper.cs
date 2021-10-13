using System.Data.Common;

namespace Masiv.Roulette.Data
{
    public interface IQueryHelper
    {
        T Create<T>(DbDataReader reader) where T : class;
    }
}