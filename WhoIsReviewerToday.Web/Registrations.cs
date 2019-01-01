using Microsoft.Extensions.DependencyInjection;
using WhoIsReviewerToday.Web.ViewModels;

namespace WhoIsReviewerToday.Web
{
    public static class Registrations
    {
        public static IServiceCollection SetupAppService(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IAppService, AppService>();

            return serviceCollection;
        }

        public static IServiceCollection SetupViewModelFactories(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IReviewViewModelFactory, ReviewViewModelFactory>();
            serviceCollection.AddScoped<IDeveloperViewModelFactory, DeveloperViewModelFactory>();

            return serviceCollection;
        }
    }
}