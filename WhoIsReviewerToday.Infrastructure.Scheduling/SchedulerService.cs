using System;
using System.Threading;
using Quartz;
using Quartz.Spi;
using WhoIsReviewerToday.Domain.Factories;
using WhoIsReviewerToday.Domain.Services;

namespace WhoIsReviewerToday.Infrastructure.Scheduling
{
    internal class SchedulerService : IDisposable, ISchedulerService
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly IJobFactory _jobFactory;
        private readonly ISchedulerFactory _schedulerFactory;

        public SchedulerService(
            ISchedulerFactory schedulerFactory,
            ICancellationTokenSourceFactory cancellationTokenSourceFactory,
            IJobFactory jobFactory)
        {
            _cancellationTokenSource = cancellationTokenSourceFactory.Create();
            _schedulerFactory = schedulerFactory;
            _jobFactory = jobFactory;
        }

        public async void StartScheduler()
        {
            var scheduler = await _schedulerFactory.GetScheduler(_cancellationTokenSource.Token);
            scheduler.JobFactory = _jobFactory;

            var job = JobBuilder.Create<MyJob>()
                .WithIdentity("job1", "group1")
                .Build();

            var trigger = (ISimpleTrigger) TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")
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