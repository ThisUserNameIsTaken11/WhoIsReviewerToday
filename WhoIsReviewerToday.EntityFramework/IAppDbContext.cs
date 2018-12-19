using Microsoft.EntityFrameworkCore;
using WhoIsReviewerToday.Domain.Models;

namespace WhoIsReviewerToday.Infrastructure.EntityFramework
{
    public interface IAppDbContext
    {
        DbSet<Chat> Chats { get; }
        DbSet<Developer> Developers { get; }
    }
}