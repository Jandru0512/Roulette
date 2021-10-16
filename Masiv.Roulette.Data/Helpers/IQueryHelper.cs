using System.Collections.Generic;
using System.Data.Common;

namespace Masiv.Roulette.Data
{
    public interface IQueryHelper
    {
        T Create<T>(DbDataReader reader) where T : class;
        List<T> CreateList<T>(DbDataReader reader) where T : class;
    }
}