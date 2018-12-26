using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using WhoIsReviewerToday.Domain.Services;

namespace WhoIsReviewerToday.Bot
{
    public interface IWhoIsReviewerTodayService : IStartAndStopBotService
    {
        void SendSimpleMessage(ChatId chartId, string text);

        string GetGreetings();

        Task<User> GetBotAsync(CancellationToken cancellationToken);
    }
}