using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using WhoIsReviewerToday.Domain.Models;

namespace WhoIsReviewerToday.Domain.Repositories
{
    public interface IChatRepository
    {
        bool Contains(long telegramChatId);

        Task<bool> TryAddChatAndSaveAsync(Chat chat);

        [CanBeNull]
        Chat GetChatByTelegramChatIdOrDefault(long telegramChatId);

        bool TryRemoveChatAndSave(Chat chat);

        IEnumerable<Chat> Items { get; }
    }
}