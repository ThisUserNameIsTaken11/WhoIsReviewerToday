using Moq;
using Telegram.Bot.Types;
using WhoIsReviewerToday.Bot;
using WhoIsReviewerToday.Domain.Repositories;
using WhoIsReviewerToday.Domain.Tests.Builders;
using WhoIsReviewerToday.Infrastructure.Commands;
using Xunit;
using ChatModel = WhoIsReviewerToday.Domain.Models.Chat;

namespace WhoIsReviewerToday.Infrastructure.Tests.Commands
{
    public class StartCommandTests
    {
        public StartCommandTests()
        {
            _whoIsReviewerTodayServiceMock = new Mock<IWhoIsReviewerTodayService>();

            _chatRepositoryMock = new Mock<IChatRepository>();
            _chatRepositoryMock.Setup(
                    repository => repository.GetChatByTelegramChatIdOrDefault(It.IsAny<long>()))
                .Returns(ChatBuilder.Any);
            _chatRepositoryMock.Setup(repository => repository.Contains(It.IsAny<long>()))
                .Returns(false);
            _chatRepositoryMock.Setup(
                    repository => repository.TryAddChatAndSaveAsync(It.IsAny<ChatModel>()))
                .ReturnsAsync(true);
        }

        private readonly Mock<IWhoIsReviewerTodayService> _whoIsReviewerTodayServiceMock;
        private readonly Mock<IChatRepository> _chatRepositoryMock;

        private StartCommand CreateCommand() => new StartCommand(
            _whoIsReviewerTodayServiceMock.Object,
            _chatRepositoryMock.Object);

        private static Message CreateMessage(
            long telegramChatId,
            string userName,
            int telegramUserId = 123) =>
            new Message
            {
                Chat = new Chat
                {
                    Id = telegramChatId
                },
                From = new User
                {
                    Id = telegramUserId,
                    Username = userName
                }
            };

        [Fact]
        public void DoesNotSendMessageAboutErrorWhenTryUpdateDeveloperAndSaveReturnsTrue()
        {
            const int telegramChatId = 234;
            const string userName = "@userName";
            var message = CreateMessage(telegramChatId, userName);

            var command = CreateCommand();
            command.Execute(message);

            _whoIsReviewerTodayServiceMock.Verify(
                repository => repository.SendSimpleMessage(
                    It.Is<ChatId>(id => id.Identifier == telegramChatId),
                    "Something goes wrong! Please ask admins and try again later"),
                Times.Never);
        }

        [Fact]
        public void DoesNotTryAddChatAndSaveWhenChatRepositoryContains()
        {
            const int telegramChatId = 234;
            const string userName = "@userName";
            _chatRepositoryMock.Setup(repository => repository.Contains(telegramChatId))
                .Returns(true);
            var message = CreateMessage(telegramChatId, userName);

            var command = CreateCommand();
            command.Execute(message);

            _chatRepositoryMock.Verify(
                repository => repository.TryAddChatAndSaveAsync(It.IsAny<ChatModel>()),
                Times.Never);
        }

        [Fact]
        public void SendsMessageWhenTryAddChatAndSaveReturnsFalse()
        {
            const int telegramChatId = 234;
            const string userName = "@userName";
            var message = CreateMessage(telegramChatId, userName);
            _chatRepositoryMock.Setup(
                    repository => repository.TryAddChatAndSaveAsync(It.IsAny<ChatModel>()))
                .ReturnsAsync(false);

            var command = CreateCommand();
            command.Execute(message);

            _whoIsReviewerTodayServiceMock.Verify(
                repository => repository.SendSimpleMessage(
                    It.Is<ChatId>(id => id.Identifier == telegramChatId),
                    "Something goes wrong! Please ask admins and try again later"),
                Times.Once);
        }

        [Fact]
        public void SendsMessageWhenTryUpdateDeveloperAndSaveReturnsTrue()
        {
            const int telegramChatId = 234;
            const string userName = "@userName";
            var message = CreateMessage(telegramChatId, userName);

            var command = CreateCommand();
            command.Execute(message);

            _whoIsReviewerTodayServiceMock.Verify(
                repository => repository.SendSimpleMessage(
                    It.Is<ChatId>(id => id.Identifier == telegramChatId),
                    "I am glad to welcome you! This chat has been added to the repository and I'll be following you"),
                Times.Once);
        }

        [Fact]
        public void TriesAddChatAndSaveAsync()
        {
            const int telegramChatId = 234;
            const string userName = "@userName";
            var message = CreateMessage(telegramChatId, userName);

            var command = CreateCommand();
            command.Execute(message);

            _chatRepositoryMock.Verify(
                repository => repository.TryAddChatAndSaveAsync(
                    It.Is<ChatModel>(
                        chat => chat.TelegramChatId == telegramChatId
                                && chat.UserName == userName
                                && chat.IsPrivate)),
                Times.Once);
        }
    }
}