using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WhoIsReviewerToday.Domain.Factories;
using WhoIsReviewerToday.Domain.Models;
using WhoIsReviewerToday.Domain.Repositories;
using WhoIsReviewerToday.Infrastructure.EntityFramework.DbContext;

namespace WhoIsReviewerToday.Infrastructure.EntityFramework.Repositories
{
    internal class ReviewRepository : IReviewRepository, IDisposable
    {
        private readonly IAppDbContext _appDbContext;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public ReviewRepository(
            IAppDbContext appDbContext,
            ICancellationTokenSourceFactory cancellationTokenSourceFactory)
        {
            _appDbContext = appDbContext;
            _cancellationTokenSource = cancellationTokenSourceFactory.Create();
        }

        public IEnumerable<Review> Items => _appDbContext.Reviews;

        public async Task<bool> TryAddRangeAndSaveAsync(IEnumerable<Review> reviews)
        {
            try
            {
                await _appDbContext.Reviews.AddRangeAsync(reviews, _cancellationTokenSource.Token);
                _appDbContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
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