using Microsoft.Extensions.DependencyInjection;
using WhoIsReviewerToday.Bot.Commands;
using WhoIsReviewerToday.Bot.Providers;
using WhoIsReviewerToday.Bot.Services;

namespace WhoIsReviewerToday.Bot
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection SetupWhoIsReviewerTodayService(this IServiceCollection serviceCollection, string token)
        {
            serviceCollection.AddSingleton<IWhoIsReviewerTodayBot, WhoIsReviewerTodayBot>(provider => new WhoIsReviewerTodayBot(token));
            serviceCollection.AddSingleton<IWhoIsReviewerTodayService, WhoIsReviewerTodayService>();

            return serviceCollection;
        }

        public static IServiceCollection SetupProviders(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ICommandProvider, CommandProvider>();

            return serviceCollection;
        }

        public static IServiceCollection SetupCommands(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ICommand, StartCommand>();
            serviceCollection.AddSingleton<ICommand, HelpCommand>();

            return serviceCollection;
        }
    }
}