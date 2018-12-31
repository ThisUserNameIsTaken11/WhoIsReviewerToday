using System;
using Quartz;
using Quartz.Simpl;
using Quartz.Spi;
using WhoIsReviewerToday.Domain.Services;

namespace WhoIsReviewerToday.Infrastructure.Scheduling
{
    internal class JobFactory : SimpleJobFactory
    {
        private readonly ISendMessageService _sendMessageService;
        private readonly IServiceProvider _serviceProvider;

        public JobFactory(IServiceProvider serviceProvider, ISendMessageService sendMessageService)
        {
            _serviceProvider = serviceProvider;
            _sendMessageService = sendMessageService;
        }

        public override IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler) => new MyJob(_serviceProvider, _sendMessageService);
    }
}