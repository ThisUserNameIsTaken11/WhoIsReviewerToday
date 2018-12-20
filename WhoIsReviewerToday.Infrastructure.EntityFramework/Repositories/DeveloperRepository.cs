using System;
using System.Collections.Generic;
using System.Linq;
using WhoIsReviewerToday.Domain.Models;
using WhoIsReviewerToday.Domain.Repositories;

namespace WhoIsReviewerToday.Infrastructure.EntityFramework.Repositories
{
    public class DeveloperRepository : IDeveloperRepository
    {
        private readonly Lazy<IEnumerable<Developer>> _developersLazyField;

        public DeveloperRepository(IAppDbContext appDbContext)
        {
            _developersLazyField = new Lazy<IEnumerable<Developer>>(() => appDbContext.Developers.ToArray());
        }

        public IEnumerable<Developer> Developers => _developersLazyField.Value;
    }
}