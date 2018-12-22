using System;
using System.Threading;
using WhoIsReviewerToday.Domain.Factories;

namespace WhoIsReviewerToday.Bot
{
    internal class WhoIsReviewerTodayService : IWhoIsReviewerTodayService, IDisposable
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly IWhoIsReviewerTodayBot _whoIsReviewerTodayBot;

        public WhoIsReviewerTodayService(
            IWhoIsReviewerTodayBot whoIsReviewerTodayBot,
            ICancellationTokenSourceFactory cancellationTokenSourceFactory)
        {
            _whoIsReviewerTodayBot = whoIsReviewerTodayBot;
            _cancellationTokenSource = cancellationTokenSourceFactory.Create();
            StartBot();
        }

        private void StartBot()
        {
            _whoIsReviewerTodayBot.StartReceiving(
                null,
                _cancellationTokenSource.Token);
        }

        private void StopBot()
        {
            _whoIsReviewerTodayBot.StopReceiving();
        }

        public void Dispose()
        {
            StopBot();

            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }
    }
}