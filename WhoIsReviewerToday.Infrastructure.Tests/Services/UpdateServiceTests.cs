using Moq;
using Telegram.Bot.Types;
using WhoIsReviewerToday.Infrastructure.Commands;
using WhoIsReviewerToday.Infrastructure.Providers;
using WhoIsReviewerToday.Infrastructure.Services;
using Xunit;

namespace WhoIsReviewerToday.Infrastructure.Tests.Services
{
    public class UpdateServiceTests
    {
        public UpdateServiceTests()
        {
            _commandProviderMock = new Mock<IBotCommandProvider>();
        }

        private readonly Mock<IBotCommandProvider> _commandProviderMock;

        private UpdateService CreateService() =>
            new UpdateService(
                _commandProviderMock.Object);

        private static Mock<ICommand> CreateAndSetupCommandMock(string messageText, bool value)
        {
            var commandMock = new Mock<ICommand>();
            commandMock
                .Setup(command => command.Matches(messageText))
                .Returns(value);

            return commandMock;
        }

        private static void SetupBotCommands(Mock<IBotCommandProvider> botCommandProviderMock, params ICommand[] commands)
        {
            botCommandProviderMock.Setup(provider => provider.GetBotCommands())
                .Returns(commands);
        }

        private static Message CreateMessage(string messageText) =>
            new Message
            {
                Text = messageText,
                Chat = new Chat { Id = 123, Username = "@HelloWorld" }
            };

        [Fact]
        public void GetsCommandsInCtor()
        {
            CreateService();

            _commandProviderMock.Verify(provider => provider.GetBotCommands(), Times.Once);
        }

        [Fact]
        public void ExecutesCommandsOnUpdate()
        {
            const string expectedMessageText = "expectedMessageText";
            var firstCommandMock = CreateAndSetupCommandMock(expectedMessageText, true);
            var secondCommandMock = CreateAndSetupCommandMock(expectedMessageText, true);
            SetupBotCommands(_commandProviderMock, firstCommandMock.Object, secondCommandMock.Object);
            var service = CreateService();
            var message = CreateMessage(expectedMessageText);

            var update = new Update { Message = message };
            service.Update(update);

            firstCommandMock.Verify(command => command.Execute(message), Times.Once);
            secondCommandMock.Verify(command => command.Execute(message), Times.Once);
        }

        [Fact]
        public void DoesNotExecuteCommandsOnUpdateWhenDoesNotMatch()
        {
            var commandMock = CreateAndSetupCommandMock(It.IsAny<string>(), false);
            SetupBotCommands(_commandProviderMock, commandMock.Object);
            var service = CreateService();

            var update = new Update { Message = CreateMessage("messageText") };
            service.Update(update);

            commandMock.Verify(command => command.Execute(It.IsAny<Message>()), Times.Never);
        }
    }
}