using System;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Args;
using WhoIsReviewerToday.Bot;
using WhoIsReviewerToday.Infrastructure.Services;

namespace WhoIsReviewerToday.Localhost
{
    public class LocalhostBotService : ILocalhostBotService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IWhoIsReviewerTodayBot _whoIsReviewerTodayBot;

        public LocalhostBotService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _whoIsReviewerTodayBot = serviceProvider
                .GetRequiredService<IWhoIsReviewerTodayBot>();
        }

        public void Start()
        {
            _whoIsReviewerTodayBot.StartReceiving();
            _whoIsReviewerTodayBot.OnUpdate += OnUpdate;
        }

        public void Stop()
        {
            _whoIsReviewerTodayBot.OnUpdate -= OnUpdate;
            _whoIsReviewerTodayBot.StopReceiving();
        }

        private void OnUpdate(object sender, UpdateEventArgs e)
        {
            var serviceScope = _serviceProvider.CreateScope();
            var updateService = serviceScope.ServiceProvider.GetRequiredService<IUpdateService>();
            updateService.Update(e.Update);
        }
    }
}