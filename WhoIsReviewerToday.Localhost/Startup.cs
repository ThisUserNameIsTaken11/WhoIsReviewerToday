using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WhoIsReviewerToday.Bot;
using WhoIsReviewerToday.Infrastructure;
using WhoIsReviewerToday.Infrastructure.EntityFramework;

namespace WhoIsReviewerToday.Localhost
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private void AddApplicationClassesRegistrations(IServiceCollection services)
        {
            var botToken = _configuration["BotForTest"];

            services.SetupWhoIsReviewerTodayBotAndService(botToken)
                .SetupDbContext("Server=(localdb)\\MSSQLLocalDB;Database=WhoIsReviewerToday;Trusted_Connection=True;MultipleActiveResultSets=true")
                .SetupDbInitializer()
                .SetupProviders()
                .SetupServices()
                .SetupCommands()
                .SetupFactories()
                .SetupRepositories();

            services.AddSingleton<ILocalhostBotService, LocalhostBotService>();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            AddApplicationClassesRegistrations(services);
        }

        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env)
        {
            app.Run(
                async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
        }
    }
}