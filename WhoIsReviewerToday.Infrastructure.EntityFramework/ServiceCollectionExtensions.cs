using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WhoIsReviewerToday.Domain;
using WhoIsReviewerToday.Domain.Repositories;
using WhoIsReviewerToday.Infrastructure.EntityFramework.Repositories;

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

        public static IServiceCollection SetupRepositories(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IDeveloperRepository, DeveloperRepository>();
            serviceCollection.AddScoped<IReviewRepository, ReviewRepository>();
            return serviceCollection;
        }
    }
}