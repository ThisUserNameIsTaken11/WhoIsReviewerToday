using WhoIsReviewerToday.Domain.Models;

namespace WhoIsReviewerToday.Domain.Tests.Builders
{
    public class ChatBuilder
    {
        public long Id { get; set; }

        public long TelegramChatId { get; set; }

        public string UserName { get; set; }

        public string FullName { get; set; }

        public Chat Build() => new Chat { Id = Id, TelegramChatId = TelegramChatId, FullName = FullName, UserName = UserName };

        public static Chat Any() => new ChatBuilder().Build();
    }
}