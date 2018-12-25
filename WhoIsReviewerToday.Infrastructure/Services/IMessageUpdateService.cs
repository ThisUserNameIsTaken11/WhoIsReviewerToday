using Telegram.Bot.Types;

namespace WhoIsReviewerToday.Infrastructure.Services
{
    public interface IMessageUpdateService
    {
        void ProcessMessage(Message message);
    }
}