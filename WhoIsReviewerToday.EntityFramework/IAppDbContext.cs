using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WhoIsReviewerToday.Domain.Models;

namespace WhoIsReviewerToday.Infrastructure.EntityFramework
{
    public interface IAppDbContext
    {
        DbSet<Chat> Chats { get; }
        DbSet<Developer> Developers { get; }

        void AddRange(IEnumerable<object> entities);

        int SaveChanges();
    }
}