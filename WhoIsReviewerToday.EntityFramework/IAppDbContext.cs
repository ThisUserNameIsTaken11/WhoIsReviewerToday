using Microsoft.EntityFrameworkCore;
using WhoIsReviewerToday.EntityFramework.Models;

namespace WhoIsReviewerToday.EntityFramework
{
    public interface IAppDbContext
    {
        DbSet<Chat> Chats { get; set; }
    }
}