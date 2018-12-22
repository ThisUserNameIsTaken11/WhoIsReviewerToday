using System.Threading;
using FluentAssertions;
using Moq;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using WhoIsReviewerToday.Domain.Factories;
using Xunit;

namespace WhoIsReviewerToday.Bot.Tests
{
    public class WhoIsReviewerTodayServiceTests
    {
        public WhoIsReviewerTodayServiceTests()
        {
            _whoIsReviewerTodayBotMock = new Mock<IWhoIsReviewerTodayBot>();
            _cancellationTokenSourceFactoryMock = new Mock<ICancellationTokenSourceFactory>();
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationTokenSourceFactoryMock.Setup(factory => factory.Create()).Returns(_cancellationTokenSource);
        }

        private readonly Mock<IWhoIsReviewerTodayBot> _whoIsReviewerTodayBotMock;
        private readonly Mock<ICancellationTokenSourceFactory> _cancellationTokenSourceFactoryMock;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly string _websiteUrl = "https://helloworld.com:8080/";

        private WhoIsReviewerTodayService CreateService() =>
            new WhoIsReviewerTodayService(_whoIsReviewerTodayBotMock.Object, _cancellationTokenSourceFactoryMock.Object);

        [Fact]
        public void SetsWebHookOnStart()
        {
            var service = CreateService();

            const string webHookUrl = "https://helloworld.com:443/api/update";
            service.StartBot(_websiteUrl);

            _whoIsReviewerTodayBotMock.Verify(
                bot => bot.SetWebhookAsync(
                    webHookUrl,
                    null,
                    0,
                    null,
                    _cancellationTokenSource.Token),
                Times.Once);
        }

        [Fact]
        public void StopsReceivingOnDispose()
        {
            var service = CreateService();

            service.Dispose();

            _whoIsReviewerTodayBotMock.Verify(
                bot => bot.StopReceiving(),
                Times.Once);
        }

        [Fact]
        public void CancelsTokenOnDispose()
        {
            var service = CreateService();

            service.Dispose();

            _cancellationTokenSource.IsCancellationRequested.Should().BeTrue();
        }

        [Fact]
        public void SendsTextMessageAsyncOnSendMessage()
        {
            var service = CreateService();
            var chatId = new ChatId(123);
            const string text = "Text";

            service.SendMessage(chatId, text);

            _whoIsReviewerTodayBotMock.Verify(
                bot => bot.SendTextMessageAsync(chatId, text, ParseMode.Default, false, false, 0, null, _cancellationTokenSource.Token),
                Times.Once);
        }
    }
}