using Telegram.Bot;

namespace WhoIsReviewerToday.Bot
{
    internal class WhoIsReviewerTodayBot : TelegramBotClient, IWhoIsReviewerTodayBot
    {
        private const string AuthToken = "710490980:AAGKck8w5UoGi9dXdPpGPgXDGBWjKOs43sM";

        public WhoIsReviewerTodayBot() : base(AuthToken)
        {
        }
    }
}