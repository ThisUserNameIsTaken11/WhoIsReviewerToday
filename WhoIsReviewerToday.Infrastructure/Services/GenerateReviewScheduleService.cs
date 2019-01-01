using System;
using System.Collections.Generic;
using System.Linq;
using WhoIsReviewerToday.Common;
using WhoIsReviewerToday.Domain.Calendar;
using WhoIsReviewerToday.Domain.Models;
using WhoIsReviewerToday.Domain.Repositories;

namespace WhoIsReviewerToday.Infrastructure.Services
{
    internal class GenerateReviewScheduleService : IGenerateReviewScheduleService
    {
        private readonly IDeveloperRepository _developerRepository;
        private readonly IHolidaysCalendar _holidaysCalendar;
        private readonly IShuffleService _shuffleService;

        public GenerateReviewScheduleService(
            IDeveloperRepository developerRepository,
            IShuffleService shuffleService,
            IHolidaysCalendar holidaysCalendar)
        {
            _developerRepository = developerRepository;
            _shuffleService = shuffleService;
            _holidaysCalendar = holidaysCalendar;
        }

        public IEnumerable<Review> GenerateReviewDuties(DateTime startedDateTime, Team team)
        {
            var developers = _developerRepository.Items.Where(developer => developer.Team == team);
            var shuffledDevelopers = _shuffleService.Shuffle(developers);

            var dateTime = GetValidDate(startedDateTime);
            var reviews = new List<Review>();

            foreach (var developer in shuffledDevelopers)
            {
                var review = new Review
                {
                    DateTime = dateTime,
                    Developer = developer
                };
                dateTime = GetValidDate(dateTime.Date.AddDays(1));

                reviews.Add(review);
            }

            return reviews;
        }

        private DateTime GetValidDate(DateTime startedDateTime)
        {
            var resultDate = startedDateTime;
            while (resultDate.DayOfWeek.InRange(DayOfWeek.Sunday, DayOfWeek.Saturday)
                   || _holidaysCalendar.ExcludedDates.Contains(resultDate.Date))
                resultDate = resultDate.AddDays(1);

            return resultDate;
        }
    }
}