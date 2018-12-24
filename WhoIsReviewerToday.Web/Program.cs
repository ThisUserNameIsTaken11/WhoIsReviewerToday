using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using WhoIsReviewerToday.Bot;
using WhoIsReviewerToday.Domain;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace WhoIsReviewerToday.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

            try
            {
                logger.Debug("init main");
                var webHost = CreateWebHostBuilder(args).Build();

                using (var scope = webHost.Services.CreateScope())
                {
                    var serviceProvider = scope.ServiceProvider;

                    var dbInitializer = serviceProvider.GetRequiredService<IDbInitializer>();
                    dbInitializer.SeedIfNeeded();

                    var hostingEnvironment = serviceProvider.GetRequiredService<IHostingEnvironment>();
                    if (hostingEnvironment.IsDevelopment())
                    {
                        serviceProvider.GetRequiredService<ILocalhostBotService>().Start();
                    }
                    else
                    {
                        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                        var websiteUrl = configuration["Website:Url"];
                        serviceProvider.GetRequiredService<IWhoIsReviewerTodayService>()
                            .Start(websiteUrl);
                    }
                }

                webHost.Run();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Stopped program because of exception");
            }
            finally
            {
                LogManager.Shutdown();
            }
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging(
                    logging =>
                    {
                        logging.ClearProviders();
                        logging.SetMinimumLevel(LogLevel.Trace);
                    })
                .UseNLog()
                .UseStartup<Startup>();
    }
}