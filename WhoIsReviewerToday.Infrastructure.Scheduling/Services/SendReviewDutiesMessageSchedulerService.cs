using System;
using System.Threading;
using Quartz;
using WhoIsReviewerToday.Domain.Factories;
using WhoIsReviewerToday.Domain.Services;
using WhoIsReviewerToday.Infrastructure.Scheduling.Jobs;

namespace WhoIsReviewerToday.Infrastructure.Scheduling.Services
{
    internal class SendReviewDutiesMessageSchedulerService : IDisposable, ISendReviewDutiesMessageSchedulerService
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly ISendReviewDutiesMessageJobFactory _sendReviewDutiesMessageJobFactory;

        public SendReviewDutiesMessageSchedulerService(
            ISchedulerFactory schedulerFactory,
            ICancellationTokenSourceFactory cancellationTokenSourceFactory,
            ISendReviewDutiesMessageJobFactory sendReviewDutiesMessageJobFactory)
        {
            _cancellationTokenSource = cancellationTokenSourceFactory.Create();
            _schedulerFactory = schedulerFactory;
            _sendReviewDutiesMessageJobFactory = sendReviewDutiesMessageJobFactory;
        }

        public async void Start()
        {
            var scheduler = await _schedulerFactory.GetScheduler(_cancellationTokenSource.Token);
            scheduler.JobFactory = _sendReviewDutiesMessageJobFactory;

            var groupName = $"{nameof(SendReviewDutiesMessageJob)}group";
            var job = JobBuilder.Create<SendReviewDutiesMessageJob>()
                .WithIdentity(nameof(SendReviewDutiesMessageJob), groupName)
                .Build();

            var trigger = (ISimpleTrigger) TriggerBuilder.Create()
                .WithIdentity($"{nameof(SendReviewDutiesMessageJob)}trigger", groupName)
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(30).RepeatForever())
                //.ModifiedByCalendar("holidays")
                .Build();

            await scheduler.ScheduleJob(job, trigger);
            await scheduler.Start();
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }
    }
}