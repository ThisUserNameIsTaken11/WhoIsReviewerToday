using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NLog;
using WhoIsReviewerToday.Domain.Factories;
using WhoIsReviewerToday.Domain.Models;
using WhoIsReviewerToday.Domain.Repositories;
using WhoIsReviewerToday.Infrastructure.EntityFramework.DbContext;

namespace WhoIsReviewerToday.Infrastructure.EntityFramework.Repositories
{
    internal class ReviewRepository : IReviewRepository, IDisposable
    {
        private static readonly Logger _logger = LogManager.GetLogger(nameof(ReviewRepository), typeof(ReviewRepository));
        private readonly IAppDbContext _appDbContext;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public ReviewRepository(
            IAppDbContext appDbContext,
            ICancellationTokenSourceFactory cancellationTokenSourceFactory)
        {
            _appDbContext = appDbContext;
            _cancellationTokenSource = cancellationTokenSourceFactory.Create();
        }

        public IEnumerable<Review> Items => _appDbContext.Reviews.Include(review => review.Developer);

        public async Task<bool> TryAddRangeAndSaveAsync(IEnumerable<Review> reviews)
        {
            try
            {
                await _appDbContext.Reviews.AddRangeAsync(reviews, _cancellationTokenSource.Token);
                _appDbContext.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.Error(e);
                return false;
            }

            return true;
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }
    }
}