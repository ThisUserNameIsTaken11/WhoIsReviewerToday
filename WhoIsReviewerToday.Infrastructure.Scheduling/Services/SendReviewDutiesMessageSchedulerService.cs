using System;
using System.Threading;
using Quartz;
using Quartz.Impl.Calendar;
using WhoIsReviewerToday.Domain.Calendar;
using WhoIsReviewerToday.Domain.Factories;
using WhoIsReviewerToday.Domain.Services;
using WhoIsReviewerToday.Infrastructure.Scheduling.Jobs;

namespace WhoIsReviewerToday.Infrastructure.Scheduling.Services
{
    internal class SendReviewDutiesMessageSchedulerService : IDisposable, ISendReviewDutiesMessageSchedulerService
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly IHolidaysCalendar _holidaysCalendar;
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly ISendReviewDutiesMessageJobFactory _sendReviewDutiesMessageJobFactory;

        public SendReviewDutiesMessageSchedulerService(
            ISchedulerFactory schedulerFactory,
            ICancellationTokenSourceFactory cancellationTokenSourceFactory,
            ISendReviewDutiesMessageJobFactory sendReviewDutiesMessageJobFactory,
            IHolidaysCalendar holidaysCalendar)
        {
            _cancellationTokenSource = cancellationTokenSourceFactory.Create();
            _schedulerFactory = schedulerFactory;
            _sendReviewDutiesMessageJobFactory = sendReviewDutiesMessageJobFactory;
            _holidaysCalendar = holidaysCalendar;
        }

        public async void Start()
        {
            var scheduler = await _schedulerFactory.GetScheduler(_cancellationTokenSource.Token);
            scheduler.JobFactory = _sendReviewDutiesMessageJobFactory;

            var groupName = $"{nameof(SendReviewDutiesMessageJob)}group";
            var job = JobBuilder.Create<SendReviewDutiesMessageJob>()
                .WithIdentity(nameof(SendReviewDutiesMessageJob), groupName)
                .Build();

            const string calendarName = nameof(IHolidaysCalendar);
            await scheduler.AddCalendar(calendarName, CreateCalendar(), false, false, _cancellationTokenSource.Token);

            var trigger = TriggerBuilder.Create()
                .WithIdentity($"{nameof(SendReviewDutiesMessageJob)}trigger", groupName)
                .WithDailyTimeIntervalSchedule(
                    x =>
                        x.OnMondayThroughFriday()
                            .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(12, 10))
                            .EndingDailyAfterCount(1)
                            .InTimeZone(TimeZoneInfo.Utc))
                .ModifiedByCalendar(calendarName)
                .Build();

            await scheduler.ScheduleJob(job, trigger);
            await scheduler.Start();
        }

        private ICalendar CreateCalendar()
        {
            var annualCalendar = new AnnualCalendar();
            foreach (var excludedDate in _holidaysCalendar.ExcludedDates)
                annualCalendar.SetDayExcluded(excludedDate, true);

            return annualCalendar;
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }
    }
}