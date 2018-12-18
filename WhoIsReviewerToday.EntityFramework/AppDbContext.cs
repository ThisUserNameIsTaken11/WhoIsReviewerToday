using Microsoft.EntityFrameworkCore;
using WhoIsReviewerToday.EntityFramework.Models;

namespace WhoIsReviewerToday.EntityFramework
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Chat> Chats { get; set; }
    }
}