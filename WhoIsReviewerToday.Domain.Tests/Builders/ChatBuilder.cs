using WhoIsReviewerToday.Domain.Models;

namespace WhoIsReviewerToday.Domain.Tests.Builders
{
    public class ChatBuilder
    {
        public long Id { get; set; }

        public long TelegramChatId { get; set; }

        public string UserName { get; set; }

        public bool IsPrivate { get; set; }

        public Chat Build() => new Chat { Id = Id, TelegramChatId = TelegramChatId, IsPrivate = IsPrivate, UserName = UserName };

        public static Chat Any() => new ChatBuilder().Build();
    }
}