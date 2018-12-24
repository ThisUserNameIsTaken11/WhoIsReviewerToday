using System;
using Telegram.Bot.Args;
using WhoIsReviewerToday.Bot;

namespace WhoIsReviewerToday.Infrastructure.Services
{
    internal class LocalhostBotService : ILocalhostBotService, IDisposable
    {
        private readonly IUpdateService _updateService;
        private readonly IWhoIsReviewerTodayBot _whoIsReviewerTodayBot;

        public LocalhostBotService(
            IUpdateService updateService,
            IWhoIsReviewerTodayBot whoIsReviewerTodayBot)
        {
            _updateService = updateService;
            _whoIsReviewerTodayBot = whoIsReviewerTodayBot;
        }

        public void Start()
        {
            _whoIsReviewerTodayBot.OnUpdate += OnUpdate;
            
            _whoIsReviewerTodayBot.OnMessage += OnMessage;
            _whoIsReviewerTodayBot.StartReceiving();
        }

        private void OnMessage(object sender, MessageEventArgs e)
        {
            
        }

        private void OnUpdate(object sender, UpdateEventArgs e)
        {
            _updateService.Update(e.Update);
        }

        public void Dispose()
        {
            _whoIsReviewerTodayBot.StopReceiving();

            _whoIsReviewerTodayBot.OnUpdate -= OnUpdate;
            _whoIsReviewerTodayBot.OnMessage -= OnMessage;
        }
    }
}