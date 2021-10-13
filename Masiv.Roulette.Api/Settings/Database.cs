using Microsoft.Extensions.Configuration;
using System.Text;

namespace Masiv.Roulette.Api
{
    public static class Database
    {
        public static string BuildConnectionString(IConfiguration configuration)
        {
            DatabaseSettings settings = new DatabaseSettings();
            configuration.Bind("database", settings);
            StringBuilder builder = new StringBuilder();
            builder.Append($"Server={settings.Server};");
            builder.Append($"Port={settings.Port};");
            builder.Append($"Uid={settings.User};");
            builder.Append($"Pwd={settings.Password};");
            builder.Append($"Database={settings.DatabaseName};");
            if (settings.AllowUserVariables)
            {
                builder.Append($"Allow User Variables=True;");
            }

            return builder.ToString();
        }
    }
}
