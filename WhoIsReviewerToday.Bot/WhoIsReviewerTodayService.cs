using System;
using System.Threading;
using Telegram.Bot.Types;
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
        }

        public async void Start(string websiteUrl)
        {
            var webHookUrl = GetWebHookUrl(websiteUrl);
            await _whoIsReviewerTodayBot.SetWebhookAsync(webHookUrl, cancellationToken: _cancellationTokenSource.Token);
        }

        public async void SendSimpleMessage(ChatId chartId, string text)
        {
            await _whoIsReviewerTodayBot.SendTextMessageAsync(chartId, text, cancellationToken: _cancellationTokenSource.Token);
        }

        public string GetGreetings() => _whoIsReviewerTodayBot.GetGreetings();

        private static string GetWebHookUrl(string websiteUrl)
        {
            var uri = new Uri(websiteUrl);
            var uriBuilder = new UriBuilder(uri) { Port = 443, Path = "api/update" };
            return uriBuilder.ToString();
        }

        private void Stop()
        {
            _whoIsReviewerTodayBot.StopReceiving();
        }

        public void Dispose()
        {
            Stop();

            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }
    }
}