using Telegram.Bot;

namespace WhoIsReviewerToday.Bot
{
    internal class WhoIsReviewerTodayBot : TelegramBotClient, IWhoIsReviewerTodayBot
    {
        public WhoIsReviewerTodayBot(string token) : base(token)
        {
        }

        public string GetGreetings()
        {
            var result = GetMeAsync().Result;
            return $"Hello, World! I am user {result.Id} and my name is {result.FirstName}.";
        }
    }
}