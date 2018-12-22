using System.Threading;
using WhoIsReviewerToday.Domain.Factories;

namespace WhoIsReviewerToday.Infrastructure.Factories
{
    internal class CancellationTokenSourceFactory : ICancellationTokenSourceFactory
    {
        public CancellationTokenSource Create() => new CancellationTokenSource();
    }
}