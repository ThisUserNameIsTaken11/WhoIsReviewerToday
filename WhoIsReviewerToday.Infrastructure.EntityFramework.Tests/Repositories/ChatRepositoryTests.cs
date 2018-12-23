using System;
using System.Collections.Generic;
using System.Threading;
using FluentAssertions;
using Moq;
using WhoIsReviewerToday.Domain.Factories;
using WhoIsReviewerToday.Domain.Models;
using WhoIsReviewerToday.Infrastructure.EntityFramework.DbContext;
using WhoIsReviewerToday.Infrastructure.EntityFramework.Repositories;
using WhoIsReviewerToday.Infrastructure.EntityFramework.Tests.Builders;
using Xunit;

namespace WhoIsReviewerToday.Infrastructure.EntityFramework.Tests.Repositories
{
    public class ChatRepositoryTests
    {
        public ChatRepositoryTests()
        {
            _chats = new List<Chat>();
            _appDbContextMock = new Mock<IAppDbContext>();
            _appDbContextMock.Setup(context => context.Chats)
                .Returns(_chats.ToQueryableDbSet());

            _cancellationTokenSourceFactoryMock = new Mock<ICancellationTokenSourceFactory>();
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationTokenSourceFactoryMock.Setup(factory => factory.Create())
                .Returns(_cancellationTokenSource);
        }

        private readonly Mock<IAppDbContext> _appDbContextMock;
        private readonly List<Chat> _chats;
        private readonly Mock<ICancellationTokenSourceFactory> _cancellationTokenSourceFactoryMock;
        private readonly CancellationTokenSource _cancellationTokenSource;

        private ChatRepository CreateRepository() => new ChatRepository(_appDbContextMock.Object, _cancellationTokenSourceFactoryMock.Object);

        [Fact]
        public void ContainsExistingChat()
        {
            _chats.Add(new ChatBuilder { TelegramChatId = 333 }.Build());

            var repository = CreateRepository();
            var contains = repository.Contains(333);

            contains.Should().BeTrue();
        }

        [Fact]
        public void DoesNotContainIfNoTelegramChatIdInRepository()
        {
            _chats.Add(new ChatBuilder { TelegramChatId = 444 }.Build());

            var repository = CreateRepository();
            var contains = repository.Contains(333);

            contains.Should().BeFalse();
        }

        [Fact]
        public async void TriesAddChatToRepository()
        {
            var chat = ChatBuilder.Any();
            var repository = CreateRepository();

            var result = await repository.TryAddChatAndSaveAsync(chat);

            result.Should().BeTrue();
        }

        [Fact]
        public async void SavesChangesOnTryAddChat()
        {
            var chat = ChatBuilder.Any();
            var repository = CreateRepository();

            await repository.TryAddChatAndSaveAsync(chat);

            _appDbContextMock.Verify(context => context.SaveChanges(), Times.Once);
        }

        [Fact]
        public async void CannotAddChatToRepositoryWithTheSameTelegramId()
        {
            var repository = CreateRepository();
            await repository.TryAddChatAndSaveAsync(new ChatBuilder { TelegramChatId = 111 }.Build());

            var result = await repository.TryAddChatAndSaveAsync(new ChatBuilder { TelegramChatId = 111 }.Build());

            result.Should().BeFalse("because the repository cannot contain 2 or more chats with the same TelegramChatId");
        }

        [Fact]
        public void CancelsTokenOnDispose()
        {
            var repository = CreateRepository();

            repository.Dispose();

            _cancellationTokenSource.IsCancellationRequested.Should().BeTrue();
        }

        [Fact]
        public void GetsChatByTelegramChatId()
        {
            var chat = new ChatBuilder { TelegramChatId = 444 }.Build();
            _chats.Add(chat);
            var repository = CreateRepository();

            var actualChat = repository.GetChatByTelegramChatId(444);

            actualChat.Should().Be(chat);
        }

        [Fact]
        public void ThrowsExceptionWhenCannotFindDeveloperByUserName()
        {
            var chat = new ChatBuilder { TelegramChatId = 444 }.Build();
            _chats.Add(chat);
            var repository = CreateRepository();

            Action getChatByTelegramChatIdAction = () => repository.GetChatByTelegramChatId(222);

            getChatByTelegramChatIdAction.Should().Throw<InvalidOperationException>();
        }
    }
}