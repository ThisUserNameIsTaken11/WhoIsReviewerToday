using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WhoIsReviewerToday.Domain.Models;

namespace WhoIsReviewerToday.Infrastructure.EntityFramework.DbContext
{
    public interface IAppDbContext
    {
        DbSet<Chat> Chats { get; }
        DbSet<Developer> Developers { get; }
        DbSet<Review> Reviews { get; set; }

        void AddRange(IEnumerable<object> entities);

        int SaveChanges();
    }
}