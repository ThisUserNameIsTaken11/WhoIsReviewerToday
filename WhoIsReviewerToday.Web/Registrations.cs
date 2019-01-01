using Microsoft.Extensions.DependencyInjection;

namespace WhoIsReviewerToday.Web
{
    public static class Registrations
    {
        public static IServiceCollection SetupAppService(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IAppService, AppService>();

            return serviceCollection;
        }
    }
}