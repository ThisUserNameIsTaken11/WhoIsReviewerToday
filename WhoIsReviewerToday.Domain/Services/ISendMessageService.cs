using System.Threading.Tasks;

namespace WhoIsReviewerToday.Domain.Services
{
    public interface ISendMessageService
    {
        Task<bool> TrySendMessageAsync(long telegramChatId, string text);
    }
}