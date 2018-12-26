using System;
using System.Collections.Generic;
using WhoIsReviewerToday.Domain.Models;

namespace WhoIsReviewerToday.Infrastructure.Services
{
    public interface IGenerateReviewScheduleService
    {
        IEnumerable<Review> GenerateReviewDuties(DateTime startedDateTime, Team team);
    }
}