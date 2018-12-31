using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using WhoIsReviewerToday.Domain.Models;
using WhoIsReviewerToday.Domain.Repositories;
using WhoIsReviewerToday.Infrastructure.Services;
using Xunit;

namespace WhoIsReviewerToday.Infrastructure.Tests.Services
{
    public class AppointDutyOnReviewServiceTests
    {
        private readonly Mock<IGenerateReviewScheduleService> _generateReviewScheduleServiceMock;
        private readonly Mock<IReviewRepository> _reviewRepositoryMock;

        public AppointDutyOnReviewServiceTests()
        {
            _reviewRepositoryMock = new Mock<IReviewRepository>();
            _generateReviewScheduleServiceMock = new Mock<IGenerateReviewScheduleService>();

            _generateReviewScheduleServiceMock.Setup(s => s.GenerateReviewDuties(It.IsAny<DateTime>(), It.IsAny<Team>()))
                .Returns(Enumerable.Empty<Review>());
        }

        private AppointDutyOnReviewService CreateService()
            => new AppointDutyOnReviewService(_reviewRepositoryMock.Object, _generateReviewScheduleServiceMock.Object);

        [Theory]
        [InlineData(Team.Desktop)]
        [InlineData(Team.Mobile)]
        public async void GeneratesDutiesForDesktopTeamOnTryAppointDutiesForDesktopAndSaveAsync(Team team)
        {
            var service = CreateService();

            var startedDateTime = new DateTime(2018, 12, 26);
            await service.TryAppointDutiesAndSaveAsync(startedDateTime, team);

            _generateReviewScheduleServiceMock.Verify(s => s.GenerateReviewDuties(startedDateTime, team), Times.Once);
        }

        [Theory]
        [InlineData(Team.Desktop)]
        [InlineData(Team.Mobile)]
        public async void TriesAddRangeAndSaveAsyncForDesktop(Team team)
        {
            var startedDateTime = new DateTime(2018, 12, 26);
            var reviews = new List<Review> { new Review() };
            _generateReviewScheduleServiceMock.Setup(s => s.GenerateReviewDuties(startedDateTime, team))
                .Returns(reviews);
            var service = CreateService();

            await service.TryAppointDutiesAndSaveAsync(startedDateTime, team);

            _reviewRepositoryMock.Verify(repository => repository.TryAddRangeAndSaveAsync(reviews), Times.Once);
        }

        [Theory]
        [InlineData(true, Team.Desktop)]
        [InlineData(true, Team.Mobile)]
        [InlineData(false, Team.Mobile)]
        [InlineData(false, Team.Desktop)]
        public async void ReturnsResultForDesktop(bool expectedResult, Team team)
        {
            _reviewRepositoryMock.Setup(repository => repository.TryAddRangeAndSaveAsync(It.IsAny<IEnumerable<Review>>()))
                .ReturnsAsync(expectedResult);
            var service = CreateService();

            var actualResult = await service.TryAppointDutiesAndSaveAsync(new DateTime(2018, 12, 26), team);

            actualResult.Should().Be(expectedResult);
        }
    }
}