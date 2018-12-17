using System.Threading;
using Moq;
using Xunit;

namespace WhoIsReviewerToday.Bot.Tests
{
    public class WhoIsReviewerTodayServiceTests
    {
        public WhoIsReviewerTodayServiceTests()
        {
            _whoIsReviewerTodayBotMock = new Mock<IWhoIsReviewerTodayBot>();
        }

        private readonly Mock<IWhoIsReviewerTodayBot> _whoIsReviewerTodayBotMock;

        private WhoIsReviewerTodayService CreateService() =>
            new WhoIsReviewerTodayService(_whoIsReviewerTodayBotMock.Object);

        [Fact]
        public void StartsReceivingOnStart()
        {
            var service = CreateService();

            service.Start();

            _whoIsReviewerTodayBotMock.Verify(
                bot => bot.StartReceiving(null, default(CancellationToken)),
                Times.Once);
        }

        [Fact]
        public void StopsReceivingOnStop()
        {
            var service = CreateService();

            service.Start();
            service.Stop();

            _whoIsReviewerTodayBotMock.Verify(
                bot => bot.StopReceiving(),
                Times.Once);
        }
    }
}