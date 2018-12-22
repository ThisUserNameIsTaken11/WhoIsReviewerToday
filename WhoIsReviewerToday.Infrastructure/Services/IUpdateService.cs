using Telegram.Bot.Types;

namespace WhoIsReviewerToday.Infrastructure.Services
{
    public interface IUpdateService
    {
        void Update(Update update);
    }
}