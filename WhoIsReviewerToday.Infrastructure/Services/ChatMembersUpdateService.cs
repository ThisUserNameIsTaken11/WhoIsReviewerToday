using System;
using System.Linq;
using System.Threading;
using NLog;
using Telegram.Bot.Types;
using WhoIsReviewerToday.Bot;
using WhoIsReviewerToday.Common;
using WhoIsReviewerToday.Domain.Factories;
using WhoIsReviewerToday.Domain.Repositories;

namespace WhoIsReviewerToday.Infrastructure.Services
{
    internal class ChatMembersUpdateService : IChatMembersUpdateService, IDisposable
    {
        private static readonly Logger _logger = LogManager.GetLogger(nameof(ChatMembersUpdateService), typeof(ChatMembersUpdateService));
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly IChatRepository _chatRepository;
        private readonly IWhoIsReviewerTodayService _whoIsReviewerTodayService;

        public ChatMembersUpdateService(
            IWhoIsReviewerTodayService whoIsReviewerTodayService,
            IChatRepository chatRepository,
            ICancellationTokenSourceFactory cancellationTokenSourceFactory)
        {
            _whoIsReviewerTodayService = whoIsReviewerTodayService;
            _chatRepository = chatRepository;
            _cancellationTokenSource = cancellationTokenSourceFactory.Create();
        }

        public async void ProcessChatMemberLeft(Message message)
        {
            var botUser = await _whoIsReviewerTodayService.GetBotAsync(_cancellationTokenSource.Token);

            if (message.LeftChatMember.Id != botUser.Id)
                return;

            var removingChat = _chatRepository.GetChatByTelegramChatIdOrDefault(message.Chat.Id);

            if (removingChat != null
                && _chatRepository.TryRemoveChatAndSave(removingChat))
                _logger.Info($"Chat ({removingChat.Id}, {removingChat.UserName}) was removed");
        }

        public async void ProcessChatMembersAdded(Message message)
        {
            var botUser = await _whoIsReviewerTodayService.GetBotAsync(_cancellationTokenSource.Token);

            if (message.NewChatMembers.All(user => user.Id != botUser.Id))
                return;

            var chat = message.Chat.ToDomain();

            if (await _chatRepository.TryAddChatAndSaveAsync(chat))
                _logger.Info($"Chat ({chat.Id}, {chat.UserName}) was added");
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }
    }
}