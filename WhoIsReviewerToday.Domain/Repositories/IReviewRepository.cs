using System.Collections.Generic;
using System.Threading.Tasks;
using WhoIsReviewerToday.Domain.Models;

namespace WhoIsReviewerToday.Domain.Repositories
{
    public interface IReviewRepository
    {
        IEnumerable<Review> Items { get; }

        Task<bool> TryAddRangeAndSaveAsync(IEnumerable<Review> reviews);
    }
}