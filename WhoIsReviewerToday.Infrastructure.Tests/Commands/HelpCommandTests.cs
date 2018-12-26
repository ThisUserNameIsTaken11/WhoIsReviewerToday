using Moq;
using Telegram.Bot.Types;
using WhoIsReviewerToday.Bot;
using WhoIsReviewerToday.Infrastructure.Commands;
using Xunit;

namespace WhoIsReviewerToday.Infrastructure.Tests.Commands
{
    public class HelpCommandTests
    {
        public HelpCommandTests()
        {
            _whoIsReviewerTodayServiceMock = new Mock<IWhoIsReviewerTodayService>();
        }

        private readonly Mock<IWhoIsReviewerTodayService> _whoIsReviewerTodayServiceMock;

        private HelpCommand CreateCommand() => new HelpCommand(_whoIsReviewerTodayServiceMock.Object);

        [Fact]
        public void SendsMessageOnExecute()
        {
            var command = CreateCommand();

            command.Execute(new Message { Chat = new Chat { Id = 321 } });

            _whoIsReviewerTodayServiceMock.Verify(
                service => service
                    .SendSimpleMessage(It.Is<ChatId>(id => id.Identifier == 321), "I can't help you! Sorry!"));
        }
    }
}