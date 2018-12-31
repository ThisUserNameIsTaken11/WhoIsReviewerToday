using System;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using WhoIsReviewerToday.Bot;
using WhoIsReviewerToday.Domain.Services;

namespace WhoIsReviewerToday.Infrastructure.Services
{
    public class LocalhostBotService : IStartAndStopBotService, ISendMessageService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IWhoIsReviewerTodayBot _whoIsReviewerTodayBot;

        public LocalhostBotService(
            IServiceProvider serviceProvider,
            IWhoIsReviewerTodayBot whoIsReviewerTodayBot)
        {
            _serviceProvider = serviceProvider;
            _whoIsReviewerTodayBot = whoIsReviewerTodayBot;
        }

        public void SendMessage(long telegramChatId, string text)
        {
            _whoIsReviewerTodayBot.SendTextMessageAsync(new ChatId(telegramChatId), text);
        }

        public void SendMessage(string username, string text)
        {
            _whoIsReviewerTodayBot.SendTextMessageAsync(new ChatId(username), text);
        }

        public void Start(string websiteUrl)
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