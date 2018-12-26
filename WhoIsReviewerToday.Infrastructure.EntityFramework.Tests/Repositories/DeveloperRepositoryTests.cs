using System;
using System.Collections.Generic;
using System.Threading;
using FluentAssertions;
using Moq;
using WhoIsReviewerToday.Domain.Factories;
using WhoIsReviewerToday.Domain.Models;
using WhoIsReviewerToday.Domain.Tests.Builders;
using WhoIsReviewerToday.Infrastructure.EntityFramework.DbContext;
using WhoIsReviewerToday.Infrastructure.EntityFramework.Repositories;
using Xunit;

namespace WhoIsReviewerToday.Infrastructure.EntityFramework.Tests.Repositories
{
    public class DeveloperRepositoryTests
    {
        public DeveloperRepositoryTests()
        {
            _developers = new List<Developer>();
            _appDbContextMock = new Mock<IAppDbContext>();
            _appDbContextMock.Setup(context => context.Developers)
                .Returns(_developers.ToQueryableDbSet());

            _cancellationTokenSourceFactoryMock = new Mock<ICancellationTokenSourceFactory>();
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationTokenSourceFactoryMock.Setup(factory => factory.Create())
                .Returns(_cancellationTokenSource);
        }

        private readonly Mock<IAppDbContext> _appDbContextMock;
        private readonly List<Developer> _developers;
        private readonly Mock<ICancellationTokenSourceFactory> _cancellationTokenSourceFactoryMock;
        private readonly CancellationTokenSource _cancellationTokenSource;

        private DeveloperRepository CreateRepository() =>
            new DeveloperRepository(_appDbContextMock.Object, _cancellationTokenSourceFactoryMock.Object);

        [Fact]
        public void ContainsExistingDeveloper()
        {
            _developers.Add(new DeveloperBuilder { UserName = "UserName" }.Build());

            var repository = CreateRepository();
            var contains = repository.Contains("UserName");

            contains.Should().BeTrue();
        }

        [Fact]
        public void DoesNotContainIfNoUserNameInRepository()
        {
            _developers.Add(new DeveloperBuilder { UserName = "UserName" }.Build());

            var repository = CreateRepository();
            var contains = repository.Contains("UnknownUserName");

            contains.Should().BeFalse();
        }

        [Fact]
        public async void TriesUpdateEntity()
        {
            var developer = new DeveloperBuilder { UserName = "UserName" }.Build();
            _developers.Add(developer);
            var repository = CreateRepository();

            var result = await repository.TryUpdateDeveloperAndSaveAsync(developer);

            result.Should().BeTrue();
        }

        [Fact]
        public async void SavesChangesOnTryUpdateDeveloperAndSave()
        {
            var developer = new DeveloperBuilder { UserName = "UserName" }.Build();
            _developers.Add(developer);
            var repository = CreateRepository();

            await repository.TryUpdateDeveloperAndSaveAsync(developer);

            _appDbContextMock.Verify(context => context.SaveChanges(), Times.Once);
        }

        [Fact]
        public void CancelsTokenOnDispose()
        {
            var repository = CreateRepository();

            repository.Dispose();

            _cancellationTokenSource.IsCancellationRequested.Should().BeTrue();
        }

        [Fact]
        public void GetsDeveloperByUserName()
        {
            var developer = new DeveloperBuilder { UserName = "UserName" }.Build();
            _developers.Add(developer);
            var repository = CreateRepository();

            var actualDeveloper = repository.GetDeveloperByUserName("UserName");

            actualDeveloper.Should().Be(developer);
        }

        [Fact]
        public void ThrowsExceptionWhenCannotFindDeveloperByUserName()
        {
            var developer = new DeveloperBuilder { UserName = "UserName" }.Build();
            _developers.Add(developer);
            var repository = CreateRepository();

            Action getDeveloperByUserNameAction = () => repository.GetDeveloperByUserName("UnknownUserName");

            getDeveloperByUserNameAction.Should().Throw<InvalidOperationException>();
        }
    }
}