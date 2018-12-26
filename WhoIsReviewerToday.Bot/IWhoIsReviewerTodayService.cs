using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace WhoIsReviewerToday.Bot
{
    public interface IWhoIsReviewerTodayService
    {
        void Start(string websiteUrl);

        void SendSimpleMessage(ChatId chartId, string text);

        string GetGreetings();

        void Stop();

        Task<User> GetBotAsync(CancellationToken cancellationToken);
    }
}