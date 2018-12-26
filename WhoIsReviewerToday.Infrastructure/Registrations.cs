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
            serviceCollection.AddScoped<IBotCommandProvider, BotCommandProvider>();

            return serviceCollection;
        }

        public static IServiceCollection SetupServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IUpdateService, UpdateService>();
            serviceCollection.AddScoped<IMessageUpdateService, MessageUpdateService>();
            serviceCollection.AddScoped<IChatMembersUpdateService, ChatMembersUpdateService>();
            serviceCollection.AddScoped<IShuffleService, ShuffleService>();
            serviceCollection.AddScoped<IGenerateReviewScheduleService, GenerateReviewScheduleService>();
            serviceCollection.AddScoped<IAppointDutyOnReviewService, AppointDutyOnReviewService>();

            return serviceCollection;
        }

        public static IServiceCollection SetupFactories(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ICancellationTokenSourceFactory, CancellationTokenSourceFactory>();

            return serviceCollection;
        }

        public static IServiceCollection SetupCommands(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<ICommand, StartCommand>();
            serviceCollection.AddScoped<ICommand, HelpCommand>();

            return serviceCollection;
        }
    }
}