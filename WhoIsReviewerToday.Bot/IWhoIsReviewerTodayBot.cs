using Telegram.Bot;

namespace WhoIsReviewerToday.Bot
{
    public interface IWhoIsReviewerTodayBot : ITelegramBotClient
    {
        string GetGreetings();
    }
}