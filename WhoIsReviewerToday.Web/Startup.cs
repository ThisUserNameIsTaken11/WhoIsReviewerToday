using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WhoIsReviewerToday.Bot;
using WhoIsReviewerToday.Infrastructure.EntityFramework;

namespace WhoIsReviewerToday.Web
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            var token = _configuration["BotSettings:Token"];
            var connectionString = _configuration["ConnectionStrings:DefaultConnection"];

            services.SetupWhoIsReviewerTodayService(token)
                .SetupDbContext(connectionString);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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