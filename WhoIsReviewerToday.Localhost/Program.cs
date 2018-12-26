using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using WhoIsReviewerToday.Domain;

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

            var localhostBotService = _webHostServices.GetRequiredService<ILocalhostBotService>();
            localhostBotService.Start();

            webHost.Run();

            localhostBotService.Stop();
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}