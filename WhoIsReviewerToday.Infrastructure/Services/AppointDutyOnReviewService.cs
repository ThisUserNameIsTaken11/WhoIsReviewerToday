using System;
using System.Threading.Tasks;
using WhoIsReviewerToday.Domain.Models;
using WhoIsReviewerToday.Domain.Repositories;

namespace WhoIsReviewerToday.Infrastructure.Services
{
    internal class AppointDutyOnReviewService : IAppointDutyOnReviewService
    {
        private readonly IGenerateReviewScheduleService _generateReviewScheduleService;
        private readonly IReviewRepository _reviewRepository;

        public AppointDutyOnReviewService(
            IReviewRepository reviewRepository,
            IGenerateReviewScheduleService generateReviewScheduleService)
        {
            _reviewRepository = reviewRepository;
            _generateReviewScheduleService = generateReviewScheduleService;
        }

        public async Task<bool> TryAppointDutiesAndSaveAsync(DateTime startedDateTime, Team team)
        {
            var reviews = _generateReviewScheduleService.GenerateReviewDuties(startedDateTime, team);

            return await _reviewRepository.TryAddRangeAndSaveAsync(reviews);
        }
    }
}