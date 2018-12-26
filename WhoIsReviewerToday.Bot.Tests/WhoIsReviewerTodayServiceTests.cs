using System;
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
            service.Start(_websiteUrl);

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
        public void DeletesWebHookOnDispose()
        {
            var service = CreateService();

            service.Dispose();

            _whoIsReviewerTodayBotMock.Verify(
                bot => bot.DeleteWebhookAsync(CancellationToken.None),
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

            service.SendSimpleMessage(chatId, text);

            _whoIsReviewerTodayBotMock.Verify(
                bot => bot.SendTextMessageAsync(chatId, text, ParseMode.Default, false, false, 0, null, _cancellationTokenSource.Token),
                Times.Once);
        }

        [Fact]
        public void GetsGreetings()
        {
            const string helloWorld = "Hello, World!";
            _whoIsReviewerTodayBotMock.Setup(bot => bot.GetGreetings())
                .Returns(helloWorld);
            var service = CreateService();

            var greetings = service.GetGreetings();

            greetings.Should().Be(helloWorld);
        }

        [Fact]
        public void DeletesWebHookOnStop()
        {
            var service = CreateService();

            service.Stop();

            _whoIsReviewerTodayBotMock.Verify(bot => bot.DeleteWebhookAsync(CancellationToken.None), Times.Once);
        }

        [Fact]
        public void DoesNotThrowWhenStopTwice()
        {
            var service = CreateService();

            Action action = service.Stop;
            action.Invoke();

            action.Should().NotThrow<ObjectDisposedException>();
        }

        [Fact]
        public void CallsGetMeAsyncOnGetBotAsync()
        {
            var service = CreateService();

            var cancellationToken = CancellationToken.None;
            service.GetBotAsync(cancellationToken);

            _whoIsReviewerTodayBotMock.Verify(bot => bot.GetMeAsync(cancellationToken), Times.Once);
        }
    }
}