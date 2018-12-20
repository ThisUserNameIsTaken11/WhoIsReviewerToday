using System.Collections.Generic;
using WhoIsReviewerToday.Domain.Models;

namespace WhoIsReviewerToday.Domain.Repositories
{
    public interface IReviewRepository
    {
        IEnumerable<Review> Items { get; }
    }
}