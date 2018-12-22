using Telegram.Bot.Types;

namespace WhoIsReviewerToday.Bot
{
    public interface IWhoIsReviewerTodayService
    {
        void StartBot(string websiteUrl);

        void SendMessage(ChatId chartId, string text);
    }
}