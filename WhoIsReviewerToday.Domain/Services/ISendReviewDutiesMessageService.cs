using System;
using System.Threading.Tasks;

namespace WhoIsReviewerToday.Domain.Services
{
    public interface ISendReviewDutiesMessageService
    {
        Task SendMessage(DateTime dateTime);
    }
}