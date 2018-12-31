using System;
using System.Threading.Tasks;
using WhoIsReviewerToday.Domain.Models;

namespace WhoIsReviewerToday.Domain.Providers
{
    public interface IReviewByDateProvider
    {
        Task<Review> GetReview(DateTime dateTime, Team team);
    }
}