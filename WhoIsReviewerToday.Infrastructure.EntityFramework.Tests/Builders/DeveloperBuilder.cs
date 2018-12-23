using WhoIsReviewerToday.Domain.Models;

namespace WhoIsReviewerToday.Infrastructure.EntityFramework.Tests.Builders
{
    public class DeveloperBuilder
    {
        public long Id { get; set; }

        public long TelegramChatId { get; set; }

        public string UserName { get; set; }

        public string FullName { get; set; }

        public Chat Chat { get; set; }

        public Team Team { get; set; }

        public long? TelegramUserId { get; set; }

        public Developer Build() => new Developer
            { Id = Id, Chat = Chat, FullName = FullName, UserName = UserName, Team = Team, TelegramUserId = TelegramUserId };

        public static Developer Any() => new DeveloperBuilder().Build();
    }
}