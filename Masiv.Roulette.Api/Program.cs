using Masiv.Roulette.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using System;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Masiv.Roulette.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Logger logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Debug("Init main.");
                IWebHost host = CreateWebHostBuilder(args).Build();
                RunSeeding(host);
                host.Run();
            }
            catch (Exception exception)
            {
                logger.Error(exception, "Stopped program because of an exception.");
                throw;
            }
            finally
            {
                LogManager.Shutdown();
            }
        }

        private static void RunSeeding(IWebHost host)
        {
            IServiceScopeFactory scopeFactory = host.Services.GetService<IServiceScopeFactory>();
            using (IServiceScope scope = scopeFactory.CreateScope())
            {
                SeedDb seeder = scope.ServiceProvider.GetService<SeedDb>();
                seeder.SeedAsync().Wait();
            }
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(GetLogLevel(Environment.GetEnvironmentVariable("ROULETTE_LOG_LEVEL")));
                })
                .UseNLog();

        private static LogLevel GetLogLevel(string level) =>
            level switch
            {
                LoggingLevel.Critical => LogLevel.Critical,
                LoggingLevel.Debug => LogLevel.Debug,
                LoggingLevel.Error => LogLevel.Error,
                LoggingLevel.Information => LogLevel.Information,
                LoggingLevel.None => LogLevel.None,
                LoggingLevel.Warning => LogLevel.Warning,
                _ => LogLevel.Trace,
            };
    }
}
