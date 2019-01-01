using System.Threading.Tasks;
using NLog;
using Telegram.Bot.Types;
using WhoIsReviewerToday.Domain.Repositories;
using WhoIsReviewerToday.Domain.Services;
using Chat = WhoIsReviewerToday.Domain.Models.Chat;

namespace WhoIsReviewerToday.Infrastructure.Commands
{
    public class StartCommand : SingleWordCommandBase
    {
        private static readonly Logger _logger = LogManager.GetLogger(nameof(StartCommand), typeof(StartCommand));
        private readonly IChatRepository _chatRepository;

        private readonly ISendMessageService _sendMessageService;

        public StartCommand(
            ISendMessageService sendMessageService,
            IChatRepository chatRepository)
        {
            _sendMessageService = sendMessageService;
            _chatRepository = chatRepository;
        }

        protected override string Code { get; } = "/start";

        public override async void Execute(Message message)
        {
            var telegramChatId = message.Chat.Id;
            var userName = message.From.Username;

            if (_chatRepository.Contains(telegramChatId))
            {
                _logger.Info($"The user ({userName}) has been added earlier. No need to write /start one more time.");
                return;
            }

            if (!await TryAddChatAndSaveAsync(telegramChatId, userName))
            {
                SendSomethingGoesWrongMessage(telegramChatId);
                return;
            }

            await _sendMessageService.TrySendMessageAsync(
                telegramChatId,
                "I am glad to welcome you! This chat has been added to the repository and I'll be following you");
        }

        private async void SendSomethingGoesWrongMessage(long telegramChatId)
        {
            await _sendMessageService.TrySendMessageAsync(
                telegramChatId,
                "Something goes wrong! Please ask admins and try again later");
        }

        private async Task<bool> TryAddChatAndSaveAsync(long telegramChatId, string userName)
        {
            var newChat = new Chat { TelegramChatId = telegramChatId, IsPrivate = true, UserName = userName };
            return await _chatRepository.TryAddChatAndSaveAsync(newChat);
        }
    }
}