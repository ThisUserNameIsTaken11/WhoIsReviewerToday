using Microsoft.EntityFrameworkCore;
using WhoIsReviewerToday.Domain.Models;

namespace WhoIsReviewerToday.Infrastructure.EntityFramework.DbContext
{
    public sealed class AppDbContext : Microsoft.EntityFrameworkCore.DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Chat> Chats { get; set; }
        public DbSet<Developer> Developers { get; set; }
        public DbSet<Review> Reviews { get; set; }
    }
}