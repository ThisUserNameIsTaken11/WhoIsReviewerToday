using Telegram.Bot.Types.Enums;
using WhoIsReviewerToday.Domain.Models;

namespace WhoIsReviewerToday.Common
{
    public static class ModelsExtensions
    {
        public static Chat ToDomain(this Telegram.Bot.Types.Chat chat) =>
            new Chat
            {
                IsPrivate = chat.Type == ChatType.Private,
                TelegramChatId = chat.Id,
                UserName = chat.Username
            };
    }
}