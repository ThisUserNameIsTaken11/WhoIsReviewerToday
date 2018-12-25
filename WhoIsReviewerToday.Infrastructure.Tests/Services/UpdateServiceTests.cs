using Moq;
using Telegram.Bot.Types;
using WhoIsReviewerToday.Infrastructure.Services;
using Xunit;

namespace WhoIsReviewerToday.Infrastructure.Tests.Services
{
    public class UpdateServiceTests
    {
        public UpdateServiceTests()
        {
            _messageUpdateServiceMock = new Mock<IMessageUpdateService>();
        }

        private readonly Mock<IMessageUpdateService> _messageUpdateServiceMock;

        private UpdateService CreateService() =>
            new UpdateService(
                _messageUpdateServiceMock.Object);

        [Fact]
        public void ProcessesMessageOnUpdate()
        {
            var service = CreateService();
            var message = new Message();

            var update = new Update { Message = message };
            service.Update(update);

            _messageUpdateServiceMock.Verify(s => s.ProcessMessage(message), Times.Once);
        }

        [Fact]
        public void DoesNotProcessMessageOnUpdateIfNotMessageType()
        {
            var service = CreateService();

            var update = new Update { Message = null };
            service.Update(update);

            _messageUpdateServiceMock.Verify(s => s.ProcessMessage(It.IsAny<Message>()), Times.Never);
        }
    }
}