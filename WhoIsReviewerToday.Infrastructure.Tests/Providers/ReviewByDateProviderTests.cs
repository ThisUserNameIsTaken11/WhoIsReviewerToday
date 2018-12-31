using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using WhoIsReviewerToday.Domain.Models;
using WhoIsReviewerToday.Domain.Repositories;
using WhoIsReviewerToday.Domain.Tests.Builders;
using WhoIsReviewerToday.Infrastructure.Providers;
using WhoIsReviewerToday.Infrastructure.Services;
using Xunit;

namespace WhoIsReviewerToday.Infrastructure.Tests.Providers
{
    public class ReviewByDateProviderTests
    {
        public ReviewByDateProviderTests()
        {
            _appointDutyOnReviewServiceMock = new Mock<IAppointDutyOnReviewService>();
            _reviewRepositoryMock = new Mock<IReviewRepository>();

            _reviews = new List<Review>();
            _reviewRepositoryMock.Setup(repository => repository.Items).Returns(_reviews);
        }

        private readonly Mock<IAppointDutyOnReviewService> _appointDutyOnReviewServiceMock;
        private readonly Mock<IReviewRepository> _reviewRepositoryMock;
        private readonly List<Review> _reviews;

        private ReviewByDateProvider CreateProvider() =>
            new ReviewByDateProvider(_appointDutyOnReviewServiceMock.Object, _reviewRepositoryMock.Object);

        [Theory]
        [InlineData(Team.Desktop)]
        [InlineData(Team.Mobile)]
        public async void CallsGetReview(Team team)
        {
            var expectedReview = new Review
                { DateTime = new DateTime(2018, 12, 31), Developer = new DeveloperBuilder { Team = team }.Build() };
            _reviews.Add(expectedReview);
            var provider = CreateProvider();

            var actualReview = await provider.GetReview(new DateTime(2018, 12, 31), team);

            actualReview.Should().Be(expectedReview);
        }

        [Fact]
        public async void CallsTryAppointDutiesAndSaveAsyncOnGetReviewWhenNothingAppointed()
        {
            var provider = CreateProvider();

            var dateTime = new DateTime(2018, 12, 31);
            const Team team = Team.Desktop;

            await provider.GetReview(dateTime, team);

            _appointDutyOnReviewServiceMock.Verify(service => service.TryAppointDutiesAndSaveAsync(dateTime, team), Times.Once);
        }

        [Fact]
        public async void ReturnsNullOnGetReviewWhenNothingAppointed()
        {
            var provider = CreateProvider();

            var dateTime = new DateTime(2018, 12, 31);
            const Team team = Team.Desktop;

            var review = await provider.GetReview(dateTime, team);

            review.Should().BeNull();
        }

        [Fact]
        public async void ReturnsReviewOnGetReviewWhenNothingAppointed()
        {
            var dateTime = new DateTime(2018, 12, 31);
            const Team team = Team.Desktop;
            var tryAppointDutiesAndSaveTask = new Task<bool>(() => true);
            _appointDutyOnReviewServiceMock.Setup(service => service.TryAppointDutiesAndSaveAsync(dateTime, team))
                .Returns(tryAppointDutiesAndSaveTask);
            var expectedReview = new Review
                { DateTime = dateTime, Developer = new DeveloperBuilder { Team = Team.Desktop }.Build() };
            var provider = CreateProvider();

            var getReviewTask = provider.GetReview(dateTime, team);
            _reviews.Add(expectedReview);
            tryAppointDutiesAndSaveTask.Start();
            var actualReview = await getReviewTask;

            actualReview.Should().Be(expectedReview);
        }
    }
}