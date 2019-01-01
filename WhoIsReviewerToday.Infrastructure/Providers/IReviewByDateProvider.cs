using System;
using System.Threading.Tasks;
using WhoIsReviewerToday.Domain.Models;

namespace WhoIsReviewerToday.Infrastructure.Providers
{
    public interface IReviewByDateProvider
    {
        Task<Review> GetReview(DateTime dateTime, Team team);
    }
}