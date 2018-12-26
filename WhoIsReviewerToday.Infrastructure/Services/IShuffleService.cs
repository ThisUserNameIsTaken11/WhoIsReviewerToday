using System.Collections.Generic;

namespace WhoIsReviewerToday.Infrastructure.Services
{
    public interface IShuffleService
    {
        IEnumerable<T> Shuffle<T>(IEnumerable<T> list);
    }
}