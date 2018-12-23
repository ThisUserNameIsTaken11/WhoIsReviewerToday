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
    internal class DeveloperRepository : IDeveloperRepository, IDisposable
    {
        private static readonly Logger _logger = LogManager.GetLogger(nameof(DeveloperRepository), typeof(DeveloperRepository));
        private readonly IAppDbContext _appDbContext;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly Lazy<IEnumerable<Developer>> _itemsLazyField;

        public DeveloperRepository(
            IAppDbContext appDbContext,
            ICancellationTokenSourceFactory cancellationTokenSourceFactory)
        {
            _appDbContext = appDbContext;
            _cancellationTokenSource = cancellationTokenSourceFactory.Create();

            _itemsLazyField = new Lazy<IEnumerable<Developer>>(() => _appDbContext.Developers.ToArray());
        }

        public IEnumerable<Developer> Items => _itemsLazyField.Value;

        public bool Contains(string userName)
        {
            return _appDbContext.Developers.Any(developer => developer.UserName == userName);
        }

        public Developer GetDeveloperByUserName(string userName)
        {
            return Items.First(developer => developer.UserName == userName);
        }

        public Task<bool> TryUpdateDeveloperAndSaveAsync(Developer developer)
        {
            return Task.Factory.StartNew(() => TryUpdateChatAndSave(developer), _cancellationTokenSource.Token);
        }

        private bool TryUpdateChatAndSave(Developer developer)
        {
            if (!Contains(developer.UserName))
                return false;

            try
            {
                _appDbContext.Developers.Update(developer);
                _appDbContext.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e);
                return false;
            }
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }
    }
}