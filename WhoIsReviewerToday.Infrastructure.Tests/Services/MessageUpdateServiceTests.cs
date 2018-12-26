using Moq;
using Telegram.Bot.Types;
using WhoIsReviewerToday.Infrastructure.Commands;
using WhoIsReviewerToday.Infrastructure.Providers;
using WhoIsReviewerToday.Infrastructure.Services;
using Xunit;

namespace WhoIsReviewerToday.Infrastructure.Tests.Services
{
    public class MessageUpdateServiceTests
    {
        public MessageUpdateServiceTests()
        {
            _commandProviderMock = new Mock<IBotCommandProvider>();
            _chatMembersUpdateServiceMock = new Mock<IChatMembersUpdateService>();
        }

        private readonly Mock<IBotCommandProvider> _commandProviderMock;
        private readonly Mock<IChatMembersUpdateService> _chatMembersUpdateServiceMock;

        private MessageUpdateService CreateService() =>
            new MessageUpdateService(
                _commandProviderMock.Object,
                _chatMembersUpdateServiceMock.Object);

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
            var commandMock = CreateAndSetupCommandMock(expectedMessageText, true);
            SetupBotCommands(_commandProviderMock, commandMock.Object);
            var service = CreateService();
            var message = CreateMessage(expectedMessageText);

            service.ProcessMessage(message);

            commandMock.Verify(command => command.Execute(message), Times.Once);
        }

        [Fact]
        public void DoesNotExecuteCommandsOnUpdateWhenDoesNotMatch()
        {
            var commandMock = CreateAndSetupCommandMock(It.IsAny<string>(), false);
            SetupBotCommands(_commandProviderMock, commandMock.Object);
            var service = CreateService();

            service.ProcessMessage(CreateMessage("messageText"));

            commandMock.Verify(command => command.Execute(It.IsAny<Message>()), Times.Never);
        }

        [Fact]
        public void DoesNotExecuteTwoOrMoreMatchesCommandOnUpdate()
        {
            const string expectedMessageText = "expectedMessageText";
            var firstCommandMock = CreateAndSetupCommandMock(expectedMessageText, true);
            var secondCommandMock = CreateAndSetupCommandMock(expectedMessageText, true);
            var thirdCommandMock = CreateAndSetupCommandMock(expectedMessageText, true);
            SetupBotCommands(_commandProviderMock, firstCommandMock.Object, secondCommandMock.Object, thirdCommandMock.Object);
            var service = CreateService();
            var message = CreateMessage(expectedMessageText);

            service.ProcessMessage(message);

            secondCommandMock.Verify(command => command.Execute(message), Times.Never);
            thirdCommandMock.Verify(command => command.Execute(message), Times.Never);
        }

        [Fact]
        public void DoesNotExecuteCommandsOnUpdateWhenNotTextMessage()
        {
            var commandMock = CreateAndSetupCommandMock(It.IsAny<string>(), false);
            SetupBotCommands(_commandProviderMock, commandMock.Object);
            var service = CreateService();

            service.ProcessMessage(CreateMessage(null));

            commandMock.Verify(command => command.Execute(It.IsAny<Message>()), Times.Never);
        }

        [Fact]
        public void CallsProcessChatMemberLeft()
        {
            var commandMock = CreateAndSetupCommandMock(It.IsAny<string>(), false);
            SetupBotCommands(_commandProviderMock, commandMock.Object);
            var service = CreateService();
            var message = CreateMessage(null);
            message.LeftChatMember = new User();

            service.ProcessMessage(message);

            _chatMembersUpdateServiceMock.Verify(s => s.ProcessChatMemberLeft(message), Times.Once);
        }

        [Fact]
        public void CallsProcessChatMembersAdded()
        {
            var commandMock = CreateAndSetupCommandMock(It.IsAny<string>(), false);
            SetupBotCommands(_commandProviderMock, commandMock.Object);
            var service = CreateService();
            var message = CreateMessage(null);
            message.NewChatMembers = new User[1];

            service.ProcessMessage(message);

            _chatMembersUpdateServiceMock.Verify(s => s.ProcessChatMembersAdded(message), Times.Once);
        }
    }
}