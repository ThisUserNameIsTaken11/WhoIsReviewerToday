using System.Collections.Generic;
using WhoIsReviewerToday.Common;

namespace WhoIsReviewerToday.Infrastructure.Services
{
    internal class ShuffleService : IShuffleService
    {
        public IEnumerable<T> Shuffle<T>(IEnumerable<T> list) => list.Shuffle();
    }
}