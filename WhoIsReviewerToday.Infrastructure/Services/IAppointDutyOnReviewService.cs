using System;
using System.Threading.Tasks;
using WhoIsReviewerToday.Domain.Models;

namespace WhoIsReviewerToday.Infrastructure.Services
{
    public interface IAppointDutyOnReviewService
    {
        Task<bool> TryAppointDutiesAndSaveAsync(DateTime startedDateTime, Team team);
    }
}