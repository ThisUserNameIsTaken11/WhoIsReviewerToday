using Moq;
using Telegram.Bot.Types;
using WhoIsReviewerToday.Domain.Services;
using WhoIsReviewerToday.Infrastructure.Commands;
using Xunit;

namespace WhoIsReviewerToday.Infrastructure.Tests.Commands
{
    public class HelpCommandTests
    {
        public HelpCommandTests()
        {
            _sendMessageServiceMock = new Mock<ISendMessageService>();
        }

        private readonly Mock<ISendMessageService> _sendMessageServiceMock;

        private HelpCommand CreateCommand() => new HelpCommand(_sendMessageServiceMock.Object);

        [Fact]
        public void SendsMessageOnExecute()
        {
            var command = CreateCommand();

            command.Execute(new Message { Chat = new Chat { Id = 321 } });

            _sendMessageServiceMock.Verify(
                service => service
                    .TrySendMessageAsync(321, "I can't help you! Sorry!"));
        }
    }
}