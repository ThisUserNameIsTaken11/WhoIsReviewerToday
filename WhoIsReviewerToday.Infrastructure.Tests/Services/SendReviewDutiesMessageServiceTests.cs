using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Moq;
using WhoIsReviewerToday.Domain.Models;
using WhoIsReviewerToday.Domain.Repositories;
using WhoIsReviewerToday.Domain.Services;
using WhoIsReviewerToday.Domain.Tests.Builders;
using WhoIsReviewerToday.Infrastructure.Providers;
using WhoIsReviewerToday.Infrastructure.Services;
using Xunit;

namespace WhoIsReviewerToday.Infrastructure.Tests.Services
{
    public class SendReviewDutiesMessageServiceTests
    {
        public SendReviewDutiesMessageServiceTests()
        {
            _sendMessageServiceMock = new Mock<ISendMessageService>();
            _reviewByDateProviderMock = new Mock<IReviewByDateProvider>();
            _chatRepositoryMock = new Mock<IChatRepository>();

            _chats = new List<Chat>();
            _chatRepositoryMock.Setup(repository => repository.Items)
                .Returns(_chats);

            _sendMessageServiceMock.Setup(s => s.TrySendMessageAsync(It.IsAny<long>(), It.IsAny<string>()))
                .ReturnsAsync(true);
        }

        private readonly Mock<IChatRepository> _chatRepositoryMock;
        private readonly List<Chat> _chats;
        private readonly Mock<IReviewByDateProvider> _reviewByDateProviderMock;
        private readonly Mock<ISendMessageService> _sendMessageServiceMock;

        private SendReviewDutiesMessageService CreateService() =>
            new SendReviewDutiesMessageService(
                _sendMessageServiceMock.Object,
                _reviewByDateProviderMock.Object,
                _chatRepositoryMock.Object);

        [Fact]
        public void SendsMessage()
        {
            const int telegramChatId = 123123;
            _chats.Add(new ChatBuilder { TelegramChatId = telegramChatId }.Build());
            var todayDate = DateTime.Today;
            var messageStringBuilder = new StringBuilder($"{todayDate:D}, review duties for: {Environment.NewLine}");
            messageStringBuilder.AppendLine($"{Team.Desktop} team - DesktopUserName");
            messageStringBuilder.AppendLine($"{Team.Mobile} team - MobileUserName");
            var message = messageStringBuilder.ToString();
            var desktopDeveloper = new DeveloperBuilder { UserName = "DesktopUserName", Team = Team.Desktop }.Build();
            var mobileDeveloper = new DeveloperBuilder { UserName = "MobileUserName", Team = Team.Mobile }.Build();
            _reviewByDateProviderMock.Setup(provider => provider.GetReview(todayDate, Team.Desktop))
                .ReturnsAsync(new Review { DateTime = todayDate, Developer = desktopDeveloper });
            _reviewByDateProviderMock.Setup(provider => provider.GetReview(todayDate, Team.Mobile))
                .ReturnsAsync(new Review { DateTime = todayDate, Developer = mobileDeveloper });
            var service = CreateService();

            service.SendMessage(todayDate);

            _sendMessageServiceMock.Verify(s => s.TrySendMessageAsync(telegramChatId, message), Times.Once);
        }

        [Fact]
        public void SendsMessageWhenCouldNotGetReviewForTeam()
        {
            const int telegramChatId = 123123;
            _chats.Add(new ChatBuilder { TelegramChatId = telegramChatId }.Build());
            var todayDate = DateTime.Today;
            var messageStringBuilder = new StringBuilder($"{todayDate:D}, review duties for: {Environment.NewLine}");
            messageStringBuilder.AppendLine($"{Team.Desktop} team - DesktopUserName");
            messageStringBuilder.AppendLine($"Could not get review duty for {Team.Mobile} team");
            var message = messageStringBuilder.ToString();
            var desktopDeveloper = new DeveloperBuilder { UserName = "DesktopUserName", Team = Team.Desktop }.Build();
            _reviewByDateProviderMock.Setup(provider => provider.GetReview(todayDate, Team.Desktop))
                .ReturnsAsync(new Review { DateTime = todayDate, Developer = desktopDeveloper });
            var service = CreateService();

            service.SendMessage(todayDate);

            _sendMessageServiceMock.Verify(s => s.TrySendMessageAsync(telegramChatId, message), Times.Once);
        }

        [Fact]
        public void RemovesChatWhenCouldNotSendMessage()
        {
            const int telegramChatId = 123123;
            var chat = new ChatBuilder { TelegramChatId = telegramChatId }.Build();
            _chats.Add(chat);
            var todayDate = DateTime.Today;
            _reviewByDateProviderMock.Setup(provider => provider.GetReview(todayDate, Team.Desktop))
                .ReturnsAsync(new Review { DateTime = todayDate, Developer = DeveloperBuilder.Any() });
            _sendMessageServiceMock.Setup(s => s.TrySendMessageAsync(telegramChatId, It.IsAny<string>()))
                .ReturnsAsync(false);
            var service = CreateService();

            service.SendMessage(todayDate);

            _chatRepositoryMock.Verify(repository => repository.TryRemoveChatAndSave(chat), Times.Once);
        }

        [Fact]
        public void DoesNotThrowExceptionWhenOneHappens()
        {
            const int telegramChatId = 123123;
            var chat = new ChatBuilder { TelegramChatId = telegramChatId }.Build();
            _chats.Add(chat);
            var todayDate = DateTime.Today;
            _reviewByDateProviderMock.Setup(provider => provider.GetReview(todayDate, Team.Desktop))
                .ThrowsAsync(new NullReferenceException());
            var service = CreateService();

            Action action = () => service.SendMessage(todayDate);

            action.Should().NotThrow();
        }
    }
}