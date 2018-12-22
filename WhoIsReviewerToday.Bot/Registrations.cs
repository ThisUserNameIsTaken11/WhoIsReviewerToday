using Microsoft.Extensions.DependencyInjection;

namespace WhoIsReviewerToday.Bot
{
    public static class Registrations
    {
        public static IServiceCollection SetupWhoIsReviewerTodayService(this IServiceCollection serviceCollection, string token)
        {
            serviceCollection.AddSingleton<IWhoIsReviewerTodayBot, WhoIsReviewerTodayBot>(provider => new WhoIsReviewerTodayBot(token));
            serviceCollection.AddSingleton<IWhoIsReviewerTodayService, WhoIsReviewerTodayService>();

            return serviceCollection;
        }
    }
}