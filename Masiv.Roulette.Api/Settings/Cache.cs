using Microsoft.Extensions.Configuration;
using System.Text;

namespace Masiv.Roulette.Api
{
    public static class Cache
    {
        public static string BuildConnectionString(IConfiguration configuration)
        {
            CacheSettings settings = new CacheSettings();
            configuration.Bind("redis", settings);
            StringBuilder builder = new StringBuilder();
            builder.Append($"{settings.Server}");
            builder.Append($":{settings.Port}");
            builder.Append($",password={settings.Password}");
            builder.Append($",abortConnect=false");

            return builder.ToString();
        }
    }
}
