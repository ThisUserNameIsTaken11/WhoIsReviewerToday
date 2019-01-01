using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using WhoIsReviewerToday.Domain.Services;

namespace WhoIsReviewerToday.Infrastructure.Scheduling.Jobs
{
    internal class SendReviewDutiesMessageJob : IJob
    {
        private readonly IServiceProvider _serviceProvider;

        public SendReviewDutiesMessageJob(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var todayDate = DateTime.Today;
            using (var scope = _serviceProvider.CreateScope())
            {
                var sendReviewDutiesMessageService = scope.ServiceProvider.GetRequiredService<ISendReviewDutiesMessageService>();
                await sendReviewDutiesMessageService.SendMessage(todayDate);
            }
        }
    }
}