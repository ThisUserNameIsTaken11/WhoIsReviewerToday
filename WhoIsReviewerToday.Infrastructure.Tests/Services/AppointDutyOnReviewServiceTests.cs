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

        [Fact]
        public async void GeneratesDutiesForDesktopTeamOnTryAppointDutiesForDesktopAndSaveAsync()
        {
            var service = CreateService();

            var startedDateTime = new DateTime(2018, 12, 26);
            await service.TryAppointDutiesForDesktopAndSaveAsync(startedDateTime);

            _generateReviewScheduleServiceMock.Verify(s => s.GenerateReviewDuties(startedDateTime, Team.Desktop), Times.Once);
        }

        [Fact]
        public async void TriesAddRangeAndSaveAsyncForDesktop()
        {
            var startedDateTime = new DateTime(2018, 12, 26);
            var reviews = new List<Review> { new Review() };
            _generateReviewScheduleServiceMock.Setup(s => s.GenerateReviewDuties(startedDateTime, Team.Desktop))
                .Returns(reviews);
            var service = CreateService();

            await service.TryAppointDutiesForDesktopAndSaveAsync(startedDateTime);

            _reviewRepositoryMock.Verify(repository => repository.TryAddRangeAndSaveAsync(reviews), Times.Once);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async void ReturnsResultForDesktop(bool expectedResult)
        {
            _reviewRepositoryMock.Setup(repository => repository.TryAddRangeAndSaveAsync(It.IsAny<IEnumerable<Review>>()))
                .ReturnsAsync(expectedResult);
            var service = CreateService();

            var actualResult = await service.TryAppointDutiesForDesktopAndSaveAsync(new DateTime(2018, 12, 26));

            actualResult.Should().Be(expectedResult);
        }

        [Fact]
        public async void GeneratesDutiesForMobileTeamOnAppointDutiesForMobileAndSaveAsync()
        {
            var service = CreateService();

            var startedDateTime = new DateTime(2018, 12, 26);
            await service.TryAppointDutiesForMobileAndSaveAsync(startedDateTime);

            _generateReviewScheduleServiceMock.Verify(s => s.GenerateReviewDuties(startedDateTime, Team.Mobile), Times.Once);
        }
        
        [Fact]
        public async void TriesAddRangeAndSaveAsyncForMobile()
        {
            var startedDateTime = new DateTime(2018, 12, 26);
            var reviews = new List<Review> { new Review() };
            _generateReviewScheduleServiceMock.Setup(s => s.GenerateReviewDuties(startedDateTime, Team.Mobile))
                .Returns(reviews);
            var service = CreateService();

            await service.TryAppointDutiesForMobileAndSaveAsync(startedDateTime);

            _reviewRepositoryMock.Verify(repository => repository.TryAddRangeAndSaveAsync(reviews), Times.Once);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async void ReturnsResultForMobile(bool expectedResult)
        {
            _reviewRepositoryMock.Setup(repository => repository.TryAddRangeAndSaveAsync(It.IsAny<IEnumerable<Review>>()))
                .ReturnsAsync(expectedResult);
            var service = CreateService();

            var actualResult = await service.TryAppointDutiesForMobileAndSaveAsync(new DateTime(2018, 12, 26));

            actualResult.Should().Be(expectedResult);
        }
    }
}