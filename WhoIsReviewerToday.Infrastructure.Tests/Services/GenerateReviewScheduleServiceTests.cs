using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using WhoIsReviewerToday.Domain.Models;
using WhoIsReviewerToday.Domain.Repositories;
using WhoIsReviewerToday.Domain.Tests.Builders;
using WhoIsReviewerToday.Infrastructure.Services;
using Xunit;

namespace WhoIsReviewerToday.Infrastructure.Tests.Services
{
    public class GenerateReviewScheduleServiceTests
    {
        private readonly Mock<IDeveloperRepository> _developerRepositoryMock;
        private readonly Dictionary<Team, IEnumerable<Developer>> _developersPerTeamDictionary;
        private readonly Mock<IShuffleService> _shuffleServiceMock;

        public GenerateReviewScheduleServiceTests()
        {
            _developerRepositoryMock = new Mock<IDeveloperRepository>();
            _shuffleServiceMock = new Mock<IShuffleService>();

            _developersPerTeamDictionary = new Dictionary<Team, IEnumerable<Developer>>
            {
                [Team.Desktop] = new List<Developer>
                {
                    new DeveloperBuilder { Team = Team.Desktop }.Build(),
                    new DeveloperBuilder { Team = Team.Desktop }.Build()
                },
                [Team.Mobile] = new List<Developer>
                {
                    new DeveloperBuilder { Team = Team.Mobile }.Build(),
                    new DeveloperBuilder { Team = Team.Mobile }.Build(),
                    new DeveloperBuilder { Team = Team.Mobile }.Build()
                }
            };

            _developerRepositoryMock.Setup(repository => repository.Items)
                .Returns(_developersPerTeamDictionary.Values.SelectMany(developers => developers));
        }

        private GenerateReviewScheduleService CreateService() =>
            new GenerateReviewScheduleService(_developerRepositoryMock.Object, _shuffleServiceMock.Object);

        [Theory]
        [InlineData(Team.Desktop)]
        [InlineData(Team.Mobile)]
        public void CallsShuffleOnGenerateReviewDuties(Team team)
        {
            var expectedDevelopers = _developersPerTeamDictionary[team];
            var service = CreateService();

            service.GenerateReviewDuties(new DateTime(2018, 12, 26), team);

            _shuffleServiceMock.Verify(s => s.Shuffle(expectedDevelopers), Times.Once);
        }

        [Theory]
        [InlineData(Team.Desktop)]
        [InlineData(Team.Mobile)]
        public void GetsReviewsCountTheSameAsDevelopersCount(Team team)
        {
            _shuffleServiceMock.Setup(s => s.Shuffle(It.IsAny<IEnumerable<Developer>>()))
                .Returns<IEnumerable<Developer>>(developers => developers);

            var expectedReviewsCount = _developersPerTeamDictionary[team].Count();
            var service = CreateService();

            var actualReviews = service.GenerateReviewDuties(new DateTime(2018, 12, 26), team);

            actualReviews.Count().Should().Be(expectedReviewsCount);
        }
    }
}