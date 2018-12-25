using System;
using System.Threading;
using NLog;
using Telegram.Bot.Types;
using WhoIsReviewerToday.Domain.Factories;

namespace WhoIsReviewerToday.Bot
{
    internal class WhoIsReviewerTodayService : IWhoIsReviewerTodayService, IDisposable
    {
        private static readonly Logger _logger = LogManager.GetLogger(nameof(WhoIsReviewerTodayService), typeof(WhoIsReviewerTodayService));
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly IWhoIsReviewerTodayBot _whoIsReviewerTodayBot;

        public WhoIsReviewerTodayService(
            IWhoIsReviewerTodayBot whoIsReviewerTodayBot,
            ICancellationTokenSourceFactory cancellationTokenSourceFactory)
        {
            _whoIsReviewerTodayBot = whoIsReviewerTodayBot;
            _cancellationTokenSource = cancellationTokenSourceFactory.Create();
        }

        public async void Start(string websiteUrl)
        {
            var webHookUrl = GetWebHookUrl(websiteUrl);
            await _whoIsReviewerTodayBot.SetWebhookAsync(webHookUrl, cancellationToken: _cancellationTokenSource.Token);

            _logger.Info($"{nameof(WhoIsReviewerTodayService)} is started with webHookUrl:{webHookUrl}");
        }

        public async void SendSimpleMessage(ChatId chartId, string text)
        {
            await _whoIsReviewerTodayBot.SendTextMessageAsync(chartId, text, cancellationToken: _cancellationTokenSource.Token);
        }

        public string GetGreetings() => _whoIsReviewerTodayBot.GetGreetings();

        public void Stop()
        {
            if (_cancellationTokenSource.IsCancellationRequested)
                return;

            _whoIsReviewerTodayBot.DeleteWebhookAsync(CancellationToken.None);

            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();

            _logger.Info($"{nameof(WhoIsReviewerTodayService)} is successfully stoped");
        }

        private static string GetWebHookUrl(string websiteUrl)
        {
            var uri = new Uri(websiteUrl);
            var uriBuilder = new UriBuilder(uri) { Port = 443, Path = "api/update" };
            return uriBuilder.ToString();
        }

        public void Dispose()
        {
            Stop();
        }
    }
}