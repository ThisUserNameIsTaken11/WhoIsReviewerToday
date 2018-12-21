using Telegram.Bot.Types;

namespace WhoIsReviewerToday.Bot.Services
{
    public interface IWhoIsReviewerTodayService
    {
        void Start();

        void Stop();

        void Update(Update update);
    }
}