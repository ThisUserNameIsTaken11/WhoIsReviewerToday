using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WhoIsReviewerToday.EntityFramework
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection SetupDbContext(this IServiceCollection serviceCollection, string connectionString)
        {
            serviceCollection.AddDbContext<IAppDbContext, AppDbContext>(builder => builder.UseSqlServer(connectionString));
            return serviceCollection;
        }
    }
}