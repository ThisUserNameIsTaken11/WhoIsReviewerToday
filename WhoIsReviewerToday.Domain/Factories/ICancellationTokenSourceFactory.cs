using System.Threading;

namespace WhoIsReviewerToday.Domain.Factories
{
    public interface ICancellationTokenSourceFactory
    {
        CancellationTokenSource Create();
    }
}