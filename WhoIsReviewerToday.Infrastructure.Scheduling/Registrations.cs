using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using WhoIsReviewerToday.Domain.Services;

namespace WhoIsReviewerToday.Infrastructure.Scheduling
{
    public static class Registrations
    {
        public static IServiceCollection SetupScheduling(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            serviceCollection.AddSingleton<IJobFactory, JobFactory>();
            serviceCollection.AddSingleton<ISchedulerService, SchedulerService>();

            return serviceCollection;
        }
    }
}