using FluentAssertions;
using Moq;
using Telegram.Bot.Types;
using WhoIsReviewerToday.Bot;
using WhoIsReviewerToday.Domain.Models;
using WhoIsReviewerToday.Domain.Repositories;
using WhoIsReviewerToday.Infrastructure.Commands;
using WhoIsReviewerToday.Infrastructure.EntityFramework.Tests.Builders;
using Xunit;
using Chat = Telegram.Bot.Types.Chat;
using ChatModel = WhoIsReviewerToday.Domain.Models.Chat;

namespace WhoIsReviewerToday.Infrastructure.Tests.Commands
{
    public class StartCommandTests
    {
        public StartCommandTests()
        {
            _whoIsReviewerTodayServiceMock = new Mock<IWhoIsReviewerTodayService>();
            _developerRepositoryMock = new Mock<IDeveloperRepository>();
            _developerRepositoryMock.Setup(repository => repository.Contains(It.IsAny<string>()))
                .Returns(true);
            _developerRepositoryMock.Setup(
                    repository => repository.GetDeveloperByUserName(It.IsAny<string>()))
                .Returns(DeveloperBuilder.Any);
            _developerRepositoryMock.Setup(repository => repository.TryUpdateDeveloperAndSaveAsync(It.IsAny<Developer>()))
                .ReturnsAsync(true);

            _chatRepositoryMock = new Mock<IChatRepository>();
            _chatRepositoryMock.Setup(
                    repository => repository.GetChatByTelegramChatId(It.IsAny<long>()))
                .Returns(ChatBuilder.Any);
            _chatRepositoryMock.Setup(repository => repository.Contains(It.IsAny<long>()))
                .Returns(false);
            _chatRepositoryMock.Setup(
                    repository => repository.TryAddChatAndSaveAsync(It.IsAny<ChatModel>()))
                .ReturnsAsync(true);

        }

        private readonly Mock<IWhoIsReviewerTodayService> _whoIsReviewerTodayServiceMock;
        private readonly Mock<IChatRepository> _chatRepositoryMock;
        private readonly Mock<IDeveloperRepository> _developerRepositoryMock;

        private StartCommand CreateCommand() => new StartCommand(
            _whoIsReviewerTodayServiceMock.Object,
            _chatRepositoryMock.Object,
            _developerRepositoryMock.Object);

        private static Message CreateMessage(
            long telegramChatId,
            string userName,
            int telegramUserId = 123,
            string firstName = "FirstName",
            string lastName = "LastName") =>
            new Message
            {
                Chat = new Chat
                {
                    Id = telegramChatId
                },
                From = new User
                {
                    Id = telegramUserId,
                    Username = userName,
                    FirstName = firstName,
                    LastName = lastName
                }
            };

        [Fact]
        public void TriesAddChatAndSaveAsync()
        {
            const int telegramChatId = 234;
            const string userName = "@userName";
            const string fullName = "Pedro Gonzalez";
            var message = CreateMessage(telegramChatId, userName, firstName: "Pedro", lastName: "Gonzalez");

            var command = CreateCommand();
            command.Execute(message);

            _chatRepositoryMock.Verify(
                repository => repository.TryAddChatAndSaveAsync(
                    It.Is<ChatModel>(
                        chat => chat.TelegramChatId == telegramChatId
                                && chat.UserName == userName
                                && chat.FullName == fullName)),
                Times.Once);
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
        public void DoesNotTryAddChatAndSaveWhenDeveloperRepositoryDoesNotContain()
        {
            const int telegramChatId = 234;
            const string userName = "@userName";
            _developerRepositoryMock.Setup(repository => repository.Contains(userName))
                .Returns(false);
            var message = CreateMessage(telegramChatId, userName);

            var command = CreateCommand();
            command.Execute(message);

            _chatRepositoryMock.Verify(
                repository => repository.TryAddChatAndSaveAsync(It.IsAny<ChatModel>()),
                Times.Never);
        }

        [Fact]
        public void SendsMessageToChatWhenDeveloperRepositoryDoesNotContain()
        {
            const long telegramChatId = 234;
            const string userName = "@userName";
            _developerRepositoryMock.Setup(repository => repository.Contains(userName))
                .Returns(false);
            var message = CreateMessage(telegramChatId, userName);

            var command = CreateCommand();
            command.Execute(message);

            _whoIsReviewerTodayServiceMock.Verify(
                repository => repository.SendSimpleMessage(
                    It.Is<ChatId>(id => id.Identifier == telegramChatId),
                    "I am not sure about you! Ask a team leader to update list of developers"),
                Times.Once);
        }

        [Fact]
        public void GetsDeveloperByUserNameWhenTriesAddChatAndSaveCompleted()
        {
            const int telegramChatId = 234;
            const string userName = "@userName";
            var message = CreateMessage(telegramChatId, userName);

            var command = CreateCommand();
            command.Execute(message);

            _developerRepositoryMock.Verify(
                repository => repository.GetDeveloperByUserName(userName),
                Times.Once);
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
        public void GetsChatByTelegramChatIdWhenTriesAddChatAndCompleted()
        {
            const int telegramChatId = 234;
            const string userName = "@userName";
            var message = CreateMessage(telegramChatId, userName);

            var command = CreateCommand();
            command.Execute(message);

            _chatRepositoryMock.Verify(
                repository => repository.GetChatByTelegramChatId(telegramChatId),
                Times.Once);
        }

        [Fact]
        public void SetsDeveloperPropertiesWhenTriesAddChatAndSaveAndGetDeveloperCompleted()
        {
            const int telegramChatId = 234;
            const string userName = "@userName";
            const string fullName = "Pedro Gonzalez";
            const int telegramUserId = 567;
            var message = CreateMessage(telegramChatId, userName, telegramUserId: telegramUserId, firstName: "Pedro", lastName: "Gonzalez");
            var developer = DeveloperBuilder.Any();
            _developerRepositoryMock.Setup(
                    repository => repository.GetDeveloperByUserName(userName))
                .Returns(developer);
            var chat = new ChatBuilder { TelegramChatId = telegramChatId }.Build();
            _chatRepositoryMock.Setup(
                    repository => repository.GetChatByTelegramChatId(telegramChatId))
                .Returns(chat);

            var command = CreateCommand();
            command.Execute(message);

            developer.Chat.Should().Be(chat);
            developer.FullName.Should().Be(fullName);
            developer.TelegramUserId.Should().Be(telegramUserId);
        }
   

        [Fact]
        public void TriesUpdateDeveloperAndSaveAsync()
        {
            const int telegramChatId = 234;
            const string userName = "@userName";
            var message = CreateMessage(telegramChatId, userName);
            var developer = DeveloperBuilder.Any();
            _developerRepositoryMock.Setup(
                    repository => repository.GetDeveloperByUserName(userName))
                .Returns(developer);

            var command = CreateCommand();
            command.Execute(message);

            _developerRepositoryMock.Verify(repository => repository.TryUpdateDeveloperAndSaveAsync(developer), Times.Once);
        }

        [Fact]
        public void SendsMessageWhenTryUpdateDeveloperAndSaveReturnsFalse()
        {
            const int telegramChatId = 234;
            const string userName = "@userName";
            var message = CreateMessage(telegramChatId, userName);
            _developerRepositoryMock.Setup(repository => repository.TryUpdateDeveloperAndSaveAsync(It.IsAny<Developer>()))
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
    }
}