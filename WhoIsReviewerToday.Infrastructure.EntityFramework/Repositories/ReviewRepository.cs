using System;
using System.Collections.Generic;
using System.Linq;
using WhoIsReviewerToday.Domain.Models;
using WhoIsReviewerToday.Domain.Repositories;
using WhoIsReviewerToday.Infrastructure.EntityFramework.DbContext;

namespace WhoIsReviewerToday.Infrastructure.EntityFramework.Repositories
{
    internal class ReviewRepository : IReviewRepository
    {
        private readonly Lazy<IEnumerable<Review>> _itemsLazyField;

        public ReviewRepository(IAppDbContext appDbContext)
        {
            _itemsLazyField = new Lazy<IEnumerable<Review>>(() => appDbContext.Reviews.ToArray());
        }

        public IEnumerable<Review> Items => _itemsLazyField.Value;
    }
}