using System;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using Telegram.Bot.Exceptions;
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

        public Task<User> GetBotAsync(CancellationToken cancellationToken) => _whoIsReviewerTodayBot.GetMeAsync(cancellationToken);

        public async void Start(string websiteUrl)
        {
            var webHookUrl = GetWebHookUrl(websiteUrl);
            await _whoIsReviewerTodayBot.SetWebhookAsync(webHookUrl, cancellationToken: _cancellationTokenSource.Token);

            _logger.Info($"{nameof(WhoIsReviewerTodayService)} is started with webHookUrl:{webHookUrl}");
        }

        public string GetGreetings() => _whoIsReviewerTodayBot.GetGreetings();

        public void Stop()
        {
            _whoIsReviewerTodayBot.DeleteWebhookAsync(CancellationToken.None);
            _logger.Info($"{nameof(WhoIsReviewerTodayService)} is successfully stoped");
        }

        public async Task<bool> TrySendMessageAsync(long telegramChatId, string text)
        {
            try
            {
                await _whoIsReviewerTodayBot.SendTextMessageAsync(
                    new ChatId(telegramChatId),
                    text,
                    cancellationToken: _cancellationTokenSource.Token);
            }
            catch (ApiRequestException)
            {
                return false;
            }

            return true;
        }

        private static string GetWebHookUrl(string websiteUrl)
        {
            var uri = new Uri(websiteUrl);
            var uriBuilder = new UriBuilder(uri) { Port = 443, Path = "api/update" };
            return uriBuilder.ToString();
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
            Stop();

            _cancellationTokenSource.Dispose();
        }
    }
}