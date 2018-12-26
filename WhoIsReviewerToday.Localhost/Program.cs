using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Args;
using WhoIsReviewerToday.Bot;
using WhoIsReviewerToday.Domain;
using WhoIsReviewerToday.Infrastructure.Services;

namespace WhoIsReviewerToday.Localhost
{
    public class Program
    {
        private static IServiceProvider _webHostServices;

        public static void Main(string[] args)
        {
            var webHost = CreateWebHostBuilder(args).Build();
            _webHostServices = webHost.Services;

            using (var scope = _webHostServices.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;

                var dbInitializer = serviceProvider.GetRequiredService<IDbInitializer>();
                dbInitializer.SeedIfNeeded();
            }

            var whoIsReviewerTodayBot = _webHostServices.GetRequiredService<IWhoIsReviewerTodayBot>();
            whoIsReviewerTodayBot.StartReceiving();
            whoIsReviewerTodayBot.OnUpdate += OnUpdate;

            webHost.Run();

            whoIsReviewerTodayBot.OnUpdate -= OnUpdate;
            whoIsReviewerTodayBot.StopReceiving();
        }

        private static void OnUpdate(object sender, UpdateEventArgs e)
        {
            using (var scope = _webHostServices.CreateScope())
            {
                var updateService = scope.ServiceProvider.GetRequiredService<IUpdateService>();
                updateService.Update(e.Update);
            }
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}