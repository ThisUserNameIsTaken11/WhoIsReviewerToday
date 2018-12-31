using System;
using System.Linq;
using System.Threading.Tasks;
using WhoIsReviewerToday.Domain.Models;
using WhoIsReviewerToday.Domain.Providers;
using WhoIsReviewerToday.Domain.Repositories;
using WhoIsReviewerToday.Infrastructure.Services;

namespace WhoIsReviewerToday.Infrastructure.Providers
{
    internal class ReviewByDateProvider : IReviewByDateProvider
    {
        private readonly IAppointDutyOnReviewService _appointDutyOnReviewService;
        private readonly IReviewRepository _reviewRepository;

        public ReviewByDateProvider(
            IAppointDutyOnReviewService appointDutyOnReviewService,
            IReviewRepository reviewRepository)
        {
            _appointDutyOnReviewService = appointDutyOnReviewService;
            _reviewRepository = reviewRepository;
        }

        public async Task<Review> GetReview(DateTime dateTime, Team team)
        {
            var review = GetReviewOrDefault(dateTime, team);
            if (review == null
                && !await _appointDutyOnReviewService.TryAppointDutiesAndSaveAsync(dateTime, team))
                return null;

            return review ?? GetReviewOrDefault(dateTime, team);
        }

        private Review GetReviewOrDefault(DateTime dateTime, Team team) => _reviewRepository.Items.FirstOrDefault(
            review => review.DateTime.Date == dateTime.Date
                      && review.Developer.Team == team);
    }
}