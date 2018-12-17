using System;

namespace WhoIsReviewerToday.Bot
{
    internal class WhoIsReviewerTodayService : IWhoIsReviewerTodayService, IDisposable
    {
        private readonly IWhoIsReviewerTodayBot _whoIsReviewerTodayBot;

        public WhoIsReviewerTodayService(IWhoIsReviewerTodayBot whoIsReviewerTodayBot)
        {
            _whoIsReviewerTodayBot = whoIsReviewerTodayBot;
        }

        public void Start()
        {
            //_whoIsReviewerTodayBot.SetWebhookAsync("");
            _whoIsReviewerTodayBot.StartReceiving();
        }

        public void Stop()
        {
            _whoIsReviewerTodayBot.StopReceiving();
        }

        public void Dispose()
        {
        }
    }
}