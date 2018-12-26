using System;
using System.Collections.Generic;
using System.Threading;
using FluentAssertions;
using Moq;
using WhoIsReviewerToday.Domain.Factories;
using WhoIsReviewerToday.Domain.Models;
using WhoIsReviewerToday.Infrastructure.EntityFramework.DbContext;
using WhoIsReviewerToday.Infrastructure.EntityFramework.Repositories;
using Xunit;

namespace WhoIsReviewerToday.Infrastructure.EntityFramework.Tests.Repositories
{
    public class ReviewRepositoryTests
    {
        public ReviewRepositoryTests()
        {
            _reviews = new List<Review>();
            _appDbContextMock = new Mock<IAppDbContext>();
            _appDbContextMock.Setup(context => context.Reviews)
                .Returns(_reviews.ToQueryableDbSet());

            _cancellationTokenSourceFactoryMock = new Mock<ICancellationTokenSourceFactory>();
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationTokenSourceFactoryMock.Setup(factory => factory.Create())
                .Returns(_cancellationTokenSource);
        }

        private readonly Mock<IAppDbContext> _appDbContextMock;
        private readonly Mock<ICancellationTokenSourceFactory> _cancellationTokenSourceFactoryMock;
        private readonly List<Review> _reviews;
        private readonly CancellationTokenSource _cancellationTokenSource;

        private ReviewRepository CreateRepository() => new ReviewRepository(
            _appDbContextMock.Object,
            _cancellationTokenSourceFactoryMock.Object);

        [Fact]
        public void CancelsTokenOnDispose()
        {
            var repository = CreateRepository();

            repository.Dispose();

            _cancellationTokenSource.IsCancellationRequested.Should().BeTrue();
        }

        [Fact]
        public async void CallsAddRangeAsyncOnTryAddRangeAndSaveAsync()
        {
            var repository = CreateRepository();
            var reviews = new List<Review> { new Review(), new Review() };

            await repository.TryAddRangeAndSaveAsync(reviews);

            _reviews.Should().BeEquivalentTo(reviews);
        }

        [Fact]
        public async void SavesChangesOnTryAddRangeAndSaveAsync()
        {
            var repository = CreateRepository();
            var reviews = new List<Review> { new Review(), new Review() };

            await repository.TryAddRangeAndSaveAsync(reviews);

            _appDbContextMock.Verify(context => context.SaveChanges(), Times.Once);
        }

        [Fact]
        public async void ReturnsFalseOnTryAddRangeAndSaveAsyncWhenExceptionHappened()
        {
            _appDbContextMock.Setup(context => context.SaveChanges()).Throws<Exception>();
            var repository = CreateRepository();

            var result = await repository.TryAddRangeAndSaveAsync(new List<Review> { new Review(), new Review() });

            result.Should().BeFalse();
        }
        
        [Fact]
        public async void ReturnsTrueOnTryAddRangeAndSaveAsyncWhenSunnyDay()
        {
            var repository = CreateRepository();

            var result = await repository.TryAddRangeAndSaveAsync(new List<Review> { new Review(), new Review() });

            result.Should().BeTrue();
        }
    }
}