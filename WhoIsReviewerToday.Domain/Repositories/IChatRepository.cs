using System.Threading.Tasks;
using WhoIsReviewerToday.Domain.Models;

namespace WhoIsReviewerToday.Domain.Repositories
{
    public interface IChatRepository
    {
        bool Contains(long telegramChatId);

        Task<bool> TryAddChatAndSaveAsync(Chat chat);

        Chat GetChatByTelegramChatId(long telegramChatId);

        bool TryRemoveChatAndSave(Chat chat);
    }
}