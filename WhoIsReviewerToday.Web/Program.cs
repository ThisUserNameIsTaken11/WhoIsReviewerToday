using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using WhoIsReviewerToday.Domain;
using WhoIsReviewerToday.Domain.Services;
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
                var webHostServices = webHost.Services;

                using (var scope = webHostServices.CreateScope())
                {
                    var serviceProvider = scope.ServiceProvider;

                    var dbInitializer = serviceProvider.GetRequiredService<IDbInitializer>();
                    dbInitializer.SeedIfNeeded();
                }

                var configuration = webHostServices.GetRequiredService<IConfiguration>();
                var websiteUrl = configuration["Website:Url"];

                var schedulerService = webHostServices.GetRequiredService<ISchedulerService>();
                var startAndStopBotService = webHostServices.GetRequiredService<IStartAndStopBotService>();
                startAndStopBotService.Start(websiteUrl);
                schedulerService.StartScheduler();

                webHost.Run();

                startAndStopBotService.Stop();
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