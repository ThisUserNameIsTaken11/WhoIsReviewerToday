using System;
using System.Threading.Tasks;

namespace WhoIsReviewerToday.Infrastructure.Services
{
    public interface IAppointDutyOnReviewService
    {
        Task<bool> TryAppointDutiesForMobileAndSaveAsync(DateTime startedDateTime);

        Task<bool> TryAppointDutiesForDesktopAndSaveAsync(DateTime startedDateTime);
    }
}