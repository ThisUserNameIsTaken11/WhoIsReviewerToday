using System;
using System.Collections.Generic;
using System.Linq;
using WhoIsReviewerToday.Domain.Models;
using WhoIsReviewerToday.Domain.Repositories;

namespace WhoIsReviewerToday.Infrastructure.Services
{
    internal class GenerateReviewScheduleService : IGenerateReviewScheduleService
    {
        private readonly IDeveloperRepository _developerRepository;
        private readonly IShuffleService _shuffleService;

        public GenerateReviewScheduleService(
            IDeveloperRepository developerRepository,
            IShuffleService shuffleService)
        {
            _developerRepository = developerRepository;
            _shuffleService = shuffleService;
        }

        public IEnumerable<Review> GenerateReviewDuties(DateTime startedDateTime, Team team)
        {
            var developers = _developerRepository.Items.Where(developer => developer.Team == team);
            var shuffledDevelopers = _shuffleService.Shuffle(developers);

            var dateTime = startedDateTime;

            var reviews = new List<Review>();

            foreach (var developer in shuffledDevelopers)
            {
                var review = new Review
                {
                    DateTime = dateTime,
                    Developer = developer
                };
                dateTime = dateTime.Date.AddDays(1);

                reviews.Add(review);
            }

            return reviews;
        }
    }
}