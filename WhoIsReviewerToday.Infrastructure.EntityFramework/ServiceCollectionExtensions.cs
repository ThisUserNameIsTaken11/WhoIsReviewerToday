using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WhoIsReviewerToday.Domain;

namespace WhoIsReviewerToday.Infrastructure.EntityFramework
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection SetupDbContext(this IServiceCollection serviceCollection, string connectionString)
        {
            serviceCollection.AddDbContext<IAppDbContext, AppDbContext>(builder => builder.UseSqlServer(connectionString));
            return serviceCollection;
        }

        public static IServiceCollection SetupDbInitializer(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IDbInitializer, DbInitializer>();
            return serviceCollection;
        }
    }
}