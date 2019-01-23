using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using WhoIsReviewerToday.Domain.Models;
using WhoIsReviewerToday.Domain.Repositories;
using WhoIsReviewerToday.Web.ViewModels;
using Xunit;

namespace WhoIsReviewerToday.Web.Tests.ViewModels
{
    public class ReviewListViewModelTests
    {
        public ReviewListViewModelTests()
        {
            _reviewRepositoryMock = new Mock<IReviewRepository>();
            _reviews = new List<Review>();
            _reviewRepositoryMock.Setup(repository => repository.Items)
                .Returns(_reviews);
            _reviewViewModelFactoryMock = new Mock<IReviewViewModelFactory>();
            _developerViewModelFactoryMock = new Mock<IDeveloperViewModelFactory>();
        }

        private readonly Mock<IDeveloperViewModelFactory> _developerViewModelFactoryMock;

        private readonly Mock<IReviewRepository> _reviewRepositoryMock;
        private readonly List<Review> _reviews;
        private readonly Mock<IReviewViewModelFactory> _reviewViewModelFactoryMock;

        private ReviewRowListViewModel CreateViewModel() =>
            new ReviewRowListViewModel(
                _reviewRepositoryMock.Object,
                _reviewViewModelFactoryMock.Object,
                _developerViewModelFactoryMock.Object
            );

        [Fact]
        public void InitializesHeadersInCtor()
        {
            var expectedHeaders = new[] { "Date" }.Concat(Enum.GetValues(typeof(Team)).Cast<Team>().Select(team => team.ToString()));
            var viewModel = CreateViewModel();

            viewModel.Headers.Should().BeEquivalentTo(expectedHeaders);
        }
    }
}