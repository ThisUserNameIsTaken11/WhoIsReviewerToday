using System;
using Quartz;
using Quartz.Simpl;
using Quartz.Spi;

namespace WhoIsReviewerToday.Infrastructure.Scheduling.Jobs
{
    internal class SendReviewDutiesMessageJobFactory : SimpleJobFactory, ISendReviewDutiesMessageJobFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public SendReviewDutiesMessageJobFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler) => new SendReviewDutiesMessageJob(_serviceProvider);
    }
}