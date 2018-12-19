using Microsoft.EntityFrameworkCore;
using WhoIsReviewerToday.Domain.Models;

namespace WhoIsReviewerToday.Infrastructure.EntityFramework
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Chat> Chats { get; set; }
        public DbSet<Developer> Developers { get; set; }
    }
}