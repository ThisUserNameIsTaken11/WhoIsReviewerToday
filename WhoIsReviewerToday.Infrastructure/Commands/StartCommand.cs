using System.Threading.Tasks;
using NLog;
using Telegram.Bot.Types;
using WhoIsReviewerToday.Bot;
using WhoIsReviewerToday.Domain.Repositories;
using Chat = WhoIsReviewerToday.Domain.Models.Chat;

namespace WhoIsReviewerToday.Infrastructure.Commands
{
    public class StartCommand : SingleWordCommandBase
    {
        private static readonly Logger _logger = LogManager.GetLogger(nameof(StartCommand), typeof(StartCommand));

        private readonly IChatRepository _chatRepository;
        private readonly IDeveloperRepository _developerRepository;
        private readonly IWhoIsReviewerTodayService _whoIsReviewerTodayService;

        public StartCommand(
            IWhoIsReviewerTodayService whoIsReviewerTodayService,
            IChatRepository chatRepository,
            IDeveloperRepository developerRepository)
        {
            _whoIsReviewerTodayService = whoIsReviewerTodayService;
            _chatRepository = chatRepository;
            _developerRepository = developerRepository;
        }

        protected override string Code { get; } = "/start";

        public override async void Execute(Message message)
        {
            var telegramChatId = message.Chat.Id;
            var telegramUserId = message.From.Id;
            var userName = message.From.Username;
            var fullName = $"{message.From.FirstName} {message.From.LastName}";

            if (_chatRepository.Contains(telegramChatId))
            {
                _logger.Info($"The user ({userName}) has been added earlier. No need to write /start one more time.");
                return;
            }

            if (!_developerRepository.Contains(userName))
            {
                _whoIsReviewerTodayService.SendSimpleMessage(
                    new ChatId(telegramChatId),
                    "I am not sure about you! Ask a team leader to update list of developers");

                _logger.Info($"The user ({userName}) is unkown for {nameof(IDeveloperRepository)}.");
                return;
            }

            if (!await TryAddChatAndSaveAsync(telegramChatId, fullName, userName)
                || !await TryUpdateDeveloperAndSaveAsync(telegramChatId, userName, telegramUserId, fullName))
            {
                SendSomethingGoesWrongMessage(telegramChatId);
                return;
            }

            _whoIsReviewerTodayService.SendSimpleMessage(
                new ChatId(telegramChatId),
                "I am glad to welcome you! This chat has been added to the repository and I'll be following you");
        }

        private void SendSomethingGoesWrongMessage(long telegramChatId)
        {
            _whoIsReviewerTodayService.SendSimpleMessage(
                new ChatId(telegramChatId),
                "Something goes wrong! Please ask admins and try again later");
        }

        private async Task<bool> TryUpdateDeveloperAndSaveAsync(long telegramChatId, string userName, int telegramUserId, string fullName)
        {
            var chat = _chatRepository.GetChatByTelegramChatId(telegramChatId);
            var developer = _developerRepository.GetDeveloperByUserName(userName);

            developer.TelegramUserId = telegramUserId;
            developer.FullName = fullName;
            developer.Chat = chat;

            return await _developerRepository.TryUpdateDeveloperAndSaveAsync(developer);
        }

        private async Task<bool> TryAddChatAndSaveAsync(long telegramChatId, string fullName, string userName)
        {
            var newChat = new Chat { TelegramChatId = telegramChatId, FullName = fullName, UserName = userName };
            return await _chatRepository.TryAddChatAndSaveAsync(newChat);
        }
    }
}