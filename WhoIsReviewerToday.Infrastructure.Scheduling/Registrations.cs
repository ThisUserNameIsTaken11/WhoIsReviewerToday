using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using WhoIsReviewerToday.Domain.Services;
using WhoIsReviewerToday.Infrastructure.Scheduling.Jobs;
using WhoIsReviewerToday.Infrastructure.Scheduling.Services;

namespace WhoIsReviewerToday.Infrastructure.Scheduling
{
    public static class Registrations
    {
        public static IServiceCollection SetupScheduling(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            serviceCollection.AddSingleton<ISendReviewDutiesMessageJobFactory, SendReviewDutiesMessageJobFactory>();
            serviceCollection.AddSingleton<ISendReviewDutiesMessageSchedulerService, SendReviewDutiesMessageSchedulerService>();

            return serviceCollection;
        }
    }
}