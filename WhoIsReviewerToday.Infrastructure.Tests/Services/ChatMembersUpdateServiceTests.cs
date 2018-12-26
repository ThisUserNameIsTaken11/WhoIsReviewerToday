using System.Threading;
using FluentAssertions;
using Moq;
using Telegram.Bot.Types;
using WhoIsReviewerToday.Bot;
using WhoIsReviewerToday.Common;
using WhoIsReviewerToday.Domain.Factories;
using WhoIsReviewerToday.Domain.Repositories;
using WhoIsReviewerToday.Infrastructure.Services;
using Xunit;

namespace WhoIsReviewerToday.Infrastructure.Tests.Services
{
    public class ChatMembersUpdateServiceTests
    {
        public ChatMembersUpdateServiceTests()
        {
            _whoIsReviewerTodayServiceMock = new Mock<IWhoIsReviewerTodayService>();
            _chatRepositoryMock = new Mock<IChatRepository>();
            _cancellationTokenSourceFactoryMock = new Mock<ICancellationTokenSourceFactory>();
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationTokenSourceFactoryMock.Setup(factory => factory.Create())
                .Returns(_cancellationTokenSource);
            _botUser = new User
            {
                Id = 100500
            };
            _whoIsReviewerTodayServiceMock.Setup(s => s.GetBotAsync(_cancellationTokenSource.Token))
                .ReturnsAsync(_botUser);

            _chatRepositoryMock.Setup(repository => repository.GetChatByTelegramChatId(It.IsAny<long>()))
                .Returns(new Domain.Models.Chat());
        }

        private readonly Mock<IChatRepository> _chatRepositoryMock;
        private readonly Mock<IWhoIsReviewerTodayService> _whoIsReviewerTodayServiceMock;
        private readonly Mock<ICancellationTokenSourceFactory> _cancellationTokenSourceFactoryMock;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly User _botUser;

        private ChatMembersUpdateService CreateService() =>
            new ChatMembersUpdateService(
                _whoIsReviewerTodayServiceMock.Object,
                _chatRepositoryMock.Object,
                _cancellationTokenSourceFactoryMock.Object);

        private static Message CreateMessage(Chat chat, User[] newChatMembers = null, User leftChatMember = null) => new Message
            { Chat = chat, NewChatMembers = newChatMembers, LeftChatMember = leftChatMember };

        [Fact]
        public void CallsGetBotAsyncOnProcessChatMemberLeft()
        {
            var service = CreateService();

            var message = CreateMessage(new Chat(), leftChatMember: new User { Id = _botUser.Id });
            service.ProcessChatMemberLeft(message);

            _whoIsReviewerTodayServiceMock.Verify(s => s.GetBotAsync(_cancellationTokenSource.Token), Times.Once);
        }

        [Fact]
        public void GetsChatByTelegramChatIdOnProcessChatMemberLeft()
        {
            var service = CreateService();

            var message = CreateMessage(new Chat(), leftChatMember: new User { Id = _botUser.Id });
            service.ProcessChatMemberLeft(message);

            _chatRepositoryMock.Verify(repository => repository.GetChatByTelegramChatId(message.Chat.Id), Times.Once);
        }

        [Fact]
        public void DoesNotGetChatByTelegramChatIdOnProcessChatMemberLeftIfLeftMemberIsNotBot()
        {
            var service = CreateService();

            var message = CreateMessage(new Chat(), leftChatMember: new User { Id = 999 });
            service.ProcessChatMemberLeft(message);

            _chatRepositoryMock.Verify(repository => repository.GetChatByTelegramChatId(message.Chat.Id), Times.Never);
        }

        [Fact]
        public void TriesRemoveChatAndSaveOnProcessChatMemberLeft()
        {
            var removedChat = new Chat().ToDomain();
            var message = CreateMessage(new Chat { Id = 222 }, leftChatMember: new User { Id = _botUser.Id });
            _chatRepositoryMock.Setup(repository => repository.GetChatByTelegramChatId(message.Chat.Id))
                .Returns(removedChat);

            var service = CreateService();
            service.ProcessChatMemberLeft(message);

            _chatRepositoryMock.Verify(repository => repository.TryRemoveChatAndSave(removedChat), Times.Once);
        }

        [Fact]
        public void CallsGetBotAsyncOnProcessChatMembersAdded()
        {
            var service = CreateService();

            var message = CreateMessage(new Chat(), newChatMembers: new[] { new User { Id = _botUser.Id } });
            service.ProcessChatMembersAdded(message);

            _whoIsReviewerTodayServiceMock.Verify(s => s.GetBotAsync(_cancellationTokenSource.Token), Times.Once);
        }

        [Fact]
        public void TriesAddChatAndSaveAsyncOnProcessChatMembersAdded()
        {
            var service = CreateService();

            var message = CreateMessage(new Chat { Id = 234 }, newChatMembers: new[] { new User { Id = _botUser.Id } });
            service.ProcessChatMembersAdded(message);

            _chatRepositoryMock.Verify(
                repository => repository.TryAddChatAndSaveAsync(It.Is<Domain.Models.Chat>(c => c.TelegramChatId == 234)),
                Times.Once);
        }

        [Fact]
        public void DoesNotTryAddChatAndSaveAsyncOnProcessChatMembersAddedNewChatMembersDoNotContainBotId()
        {
            var service = CreateService();

            var message = CreateMessage(new Chat { Id = 234 }, newChatMembers: new[] { new User { Id = 890 } });
            service.ProcessChatMembersAdded(message);

            _chatRepositoryMock.Verify(
                repository => repository.TryAddChatAndSaveAsync(It.Is<Domain.Models.Chat>(c => c.TelegramChatId == 234)),
                Times.Never);
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