using Microsoft.Extensions.Configuration;
using Moq;
using WhoIsReviewerToday.Domain.Services;
using Xunit;

namespace WhoIsReviewerToday.Web.Tests
{
    public class AppServiceTests
    {
        public AppServiceTests()
        {
            _configurationMock = new Mock<IConfiguration>();
            _schedulerServiceMock = new Mock<ISendReviewDutiesMessageSchedulerService>();
            _startAndStopBotServiceMock = new Mock<IStartAndStopBotService>();
        }

        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<ISendReviewDutiesMessageSchedulerService> _schedulerServiceMock;
        private readonly Mock<IStartAndStopBotService> _startAndStopBotServiceMock;

        private AppService CreateService() => new AppService(
            _configurationMock.Object,
            _schedulerServiceMock.Object,
            _startAndStopBotServiceMock.Object);

        [Fact]
        public void StartsServicesOnStart()
        {
            const string websiteUrl = "websiteUrl";
            _configurationMock.Setup(configuration => configuration["Website:Url"])
                .Returns(websiteUrl);
            var service = CreateService();

            service.Start();

            _startAndStopBotServiceMock.Verify(s => s.Start(websiteUrl), Times.Once);
            _schedulerServiceMock.Verify(s => s.Start(), Times.Once);
        }

        [Fact]
        public void StopsServicesOnStop()
        {
            var service = CreateService();

            service.Stop();

            _startAndStopBotServiceMock.Verify(s => s.Stop(), Times.Once);
        }
    }
}