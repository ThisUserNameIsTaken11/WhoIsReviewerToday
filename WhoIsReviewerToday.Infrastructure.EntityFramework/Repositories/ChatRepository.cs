using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using WhoIsReviewerToday.Domain.Factories;
using WhoIsReviewerToday.Domain.Models;
using WhoIsReviewerToday.Domain.Repositories;
using WhoIsReviewerToday.Infrastructure.EntityFramework.DbContext;

namespace WhoIsReviewerToday.Infrastructure.EntityFramework.Repositories
{
    internal class ChatRepository : IChatRepository, IDisposable
    {
        private static readonly Logger _logger = LogManager.GetLogger(nameof(ChatRepository), typeof(ChatRepository));
        private readonly IAppDbContext _appDbContext;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public ChatRepository(
            IAppDbContext appDbContext,
            ICancellationTokenSourceFactory cancellationTokenSourceFactory)
        {
            _appDbContext = appDbContext;

            _cancellationTokenSource = cancellationTokenSourceFactory.Create();
        }

        public IEnumerable<Chat> Items => _appDbContext.Chats;

        public bool Contains(long telegramChatId)
        {
            return _appDbContext.Chats.Any(chat => chat.TelegramChatId == telegramChatId);
        }

        public async Task<bool> TryAddChatAndSaveAsync(Chat chat)
        {
            if (Contains(chat.TelegramChatId))
                return false;

            try
            {
                await AddChatAsync(chat);
                _appDbContext.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.Error(e);
                return false;
            }

            return true;
        }

        public Chat GetChatByTelegramChatIdOrDefault(long telegramChatId)
        {
            return _appDbContext.Chats.FirstOrDefault(chat => chat.TelegramChatId == telegramChatId);
        }

        public bool TryRemoveChatAndSave(Chat chat)
        {
            if (!Contains(chat.TelegramChatId))
                return false;

            try
            {
                RemoveChat(chat);
                _appDbContext.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.Error(e);
                return false;
            }

            return true;
        }

        private async Task AddChatAsync(Chat chat)
        {
            await _appDbContext.Chats.AddAsync(chat, _cancellationTokenSource.Token);
        }

        private void RemoveChat(Chat chat)
        {
            _appDbContext.Chats.Remove(chat);
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }
    }
}