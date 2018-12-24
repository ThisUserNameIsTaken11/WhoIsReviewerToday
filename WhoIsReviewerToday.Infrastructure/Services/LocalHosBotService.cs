using System;
using Telegram.Bot.Args;
using WhoIsReviewerToday.Bot;

namespace WhoIsReviewerToday.Infrastructure.Services
{
    internal class LocalHosBotService : ILocalHosBotService, IDisposable
    {
        private readonly IUpdateService _updateService;
        private readonly IWhoIsReviewerTodayBot _whoIsReviewerTodayBot;

        public LocalHosBotService(
            IUpdateService updateService,
            IWhoIsReviewerTodayBot whoIsReviewerTodayBot)
        {
            _updateService = updateService;
            _whoIsReviewerTodayBot = whoIsReviewerTodayBot;

            _whoIsReviewerTodayBot.OnUpdate += OnUpdate;
            _whoIsReviewerTodayBot.StartReceiving();
        }

        private void OnUpdate(object sender, UpdateEventArgs e)
        {
            _updateService.Update(e.Update);
        }

        public void Dispose()
        {
            _whoIsReviewerTodayBot.OnUpdate -= OnUpdate;
        }
    }
}