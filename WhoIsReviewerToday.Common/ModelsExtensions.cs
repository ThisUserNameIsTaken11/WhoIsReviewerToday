using WhoIsReviewerToday.Domain.Models;

namespace WhoIsReviewerToday.Common
{
    public static class ModelsExtensions
    {
        public static Chat ToDomain(this Telegram.Bot.Types.Chat chat) =>
            new Chat
            {
                FullName = $"{chat.FirstName} {chat.LastName}",
                TelegramChatId = chat.Id,
                UserName = chat.Username
            };
    }
}