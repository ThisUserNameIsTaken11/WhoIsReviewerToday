using System;
using System.Collections.Generic;
using System.Linq;
using WhoIsReviewerToday.Domain.Models;
using WhoIsReviewerToday.Domain.Repositories;

namespace WhoIsReviewerToday.Infrastructure.EntityFramework.Repositories
{
    public class DeveloperRepository : IDeveloperRepository
    {
        private readonly Lazy<IEnumerable<Developer>> _itemsLazyField;

        public DeveloperRepository(IAppDbContext appDbContext)
        {
            _itemsLazyField = new Lazy<IEnumerable<Developer>>(() => appDbContext.Developers.ToArray());
        }

        public IEnumerable<Developer> Items => _itemsLazyField.Value;
    }
}