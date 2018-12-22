using Microsoft.Extensions.DependencyInjection;
using WhoIsReviewerToday.Domain.Factories;
using WhoIsReviewerToday.Infrastructure.Commands;
using WhoIsReviewerToday.Infrastructure.Factories;
using WhoIsReviewerToday.Infrastructure.Providers;
using WhoIsReviewerToday.Infrastructure.Services;

namespace WhoIsReviewerToday.Infrastructure
{
    public static class Registrations
    {
        public static IServiceCollection SetupProviders(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IBotCommandProvider, BotCommandProvider>();

            return serviceCollection;
        }

        public static IServiceCollection SetupServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IUpdateService, UpdateService>();

            return serviceCollection;
        }

        public static IServiceCollection SetupFactories(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ICancellationTokenSourceFactory, CancellationTokenSourceFactory>();

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