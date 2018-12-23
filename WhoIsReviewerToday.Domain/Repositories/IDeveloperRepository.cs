using System.Collections.Generic;
using System.Threading.Tasks;
using WhoIsReviewerToday.Domain.Models;

namespace WhoIsReviewerToday.Domain.Repositories
{
    public interface IDeveloperRepository
    {
        IEnumerable<Developer> Items { get; }

        bool Contains(string userName);

        Task<bool> TryUpdateDeveloperAndSaveAsync(Developer developer);

        Developer GetDeveloperByUserName(string userName);
    }
}