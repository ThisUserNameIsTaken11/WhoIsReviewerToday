using Telegram.Bot.Types;

namespace WhoIsReviewerToday.Infrastructure.Services
{
    public interface IChatMembersUpdateService
    {
        void ProcessChatMemberLeft(Message message);

        void ProcessChatMembersAdded(Message message);
    }
}