using Microsoft.Extensions.DependencyInjection;
using WhoIsReviewerToday.Domain.Services;

namespace WhoIsReviewerToday.Bot
{
    public static class Registrations
    {
        public static IServiceCollection SetupWhoIsReviewerTodayBotAndService(
            this IServiceCollection serviceCollection,
            string botToken)
        {
            serviceCollection.AddSingleton<IWhoIsReviewerTodayBot, WhoIsReviewerTodayBot>(provider => new WhoIsReviewerTodayBot(botToken));
            serviceCollection.AddSingleton<IWhoIsReviewerTodayService, WhoIsReviewerTodayService>();

            return serviceCollection;
        }

        public static IServiceCollection SetupStartAndStopBotService(
            this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IStartAndStopBotService, WhoIsReviewerTodayService>();

            return serviceCollection;
        }
    }
}