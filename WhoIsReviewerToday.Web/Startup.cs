using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WhoIsReviewerToday.Bot;
using WhoIsReviewerToday.Infrastructure;
using WhoIsReviewerToday.Infrastructure.EntityFramework;
using WhoIsReviewerToday.Infrastructure.Scheduling;

namespace WhoIsReviewerToday.Web
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;

        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        private void AddApplicationClassesRegistrations(IServiceCollection services)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var botToken = _configuration["APPSETTING_BotToken"];

            services.SetupWhoIsReviewerTodayBotAndService(botToken)
                .SetupDbContext(connectionString)
                .SetupDbInitializer()
                .SetupProviders()
                .SetupServices()
                .SetupCommands()
                .SetupFactories()
                .SetupScheduling()
                .SetupRepositories();

            if (_hostingEnvironment.IsDevelopment())
                services.SetupStartAndStopBotServiceForDevelopment();
            else
                services.SetupStartAndStopBotService();
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
            if (env.IsDevelopment())
                app.UseStatusCodePages()
                    .UseDeveloperExceptionPage();
            else
                app.UseExceptionHandler("/Error");

            app.UseStaticFiles()
                .UseMvc(
                    routes =>
                    {
                        routes.MapRoute(
                            "default",
                            "{controller=Home}/{action=Index}");
                    });
        }
    }
}