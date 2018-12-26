using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;

namespace WhoIsReviewerToday.Infrastructure.EntityFramework.Tests
{
    public static class Extensions
    {
        public static DbSet<T> ToQueryableDbSet<T>(this ICollection<T> sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();

            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            dbSet.Setup(d => d.Add(It.IsAny<T>())).Callback<T>(sourceList.Add);
            dbSet
                .Setup(_ => _.AddAsync(It.IsAny<T>(), It.IsAny<CancellationToken>()))
                .Callback((T model, CancellationToken token) => { sourceList.Add(model); })
                .Returns((T model, CancellationToken token) => Task.FromResult((EntityEntry<T>) null));

            dbSet
                .Setup(_ => _.AddRangeAsync(It.IsAny<IEnumerable<T>>(), It.IsAny<CancellationToken>()))
                .Callback(
                    (IEnumerable<T> models, CancellationToken token) =>
                    {
                        foreach (var model in models)
                            sourceList.Add(model);
                    })
                .Returns((IEnumerable<T> models, CancellationToken token) => Task.FromResult((EntityEntry<T>) null));

            return dbSet.Object;
        }
    }
}