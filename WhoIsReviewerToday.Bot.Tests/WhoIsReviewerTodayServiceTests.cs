using System.Threading;
using FluentAssertions;
using Moq;
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

        private WhoIsReviewerTodayService CreateService() =>
            new WhoIsReviewerTodayService(_whoIsReviewerTodayBotMock.Object, _cancellationTokenSourceFactoryMock.Object);

        [Fact]
        public void StartsReceivingInCtor()
        {
            CreateService();

            _whoIsReviewerTodayBotMock.Verify(
                bot => bot.StartReceiving(null, _cancellationTokenSource.Token),
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
    }
}