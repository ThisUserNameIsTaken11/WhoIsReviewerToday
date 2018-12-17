using System;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace WhoIsReviewerToday.Bot
{
    internal class WhoIsReviewerTodayService : IWhoIsReviewerTodayService, IDisposable
    {
        private readonly IWhoIsReviewerTodayBot _whoIsReviewerTodayBot;

        public WhoIsReviewerTodayService(IWhoIsReviewerTodayBot whoIsReviewerTodayBot)
        {
            _whoIsReviewerTodayBot = whoIsReviewerTodayBot;
        }

        public void Dispose()
        {
        }

        public void Start()
        {
            _whoIsReviewerTodayBot.StartReceiving();
        }

        public void Stop()
        {
            _whoIsReviewerTodayBot.StopReceiving();
        }
    }
}