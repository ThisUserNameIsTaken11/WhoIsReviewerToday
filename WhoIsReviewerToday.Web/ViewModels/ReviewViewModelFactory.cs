using System;
using System.Collections.Generic;
using WhoIsReviewerToday.Domain.Repositories;

namespace WhoIsReviewerToday.Web.ViewModels
{
    internal class ReviewViewModelFactory : IReviewViewModelFactory
    {
        private readonly IDeveloperViewModelFactory _developerViewModelFactory;
        private readonly IReviewRepository _reviewRepository;

        public ReviewViewModelFactory(
            IReviewRepository reviewRepository,
            IDeveloperViewModelFactory developerViewModelFactory)
        {
            _reviewRepository = reviewRepository;
            _developerViewModelFactory = developerViewModelFactory;
        }

        public IReviewRowListViewModel CreateRowList() => new ReviewRowListViewModel(_reviewRepository, this, _developerViewModelFactory);

        public IReviewRowViewModel CreateRow(DateTime dateTime, IEnumerable<IDeveloperViewModel> developers) =>
            new ReviewRowViewModel(dateTime, developers);

        public IReviewViewModel Create(DateTime dateTime, IDeveloperViewModel developer) =>
            new ReviewViewModel
            {
                DateTime = dateTime,
                Developer = developer
            };

        public IReviewListViewModel CreateList() => new ReviewListViewModel(_reviewRepository, this, _developerViewModelFactory);
    }
}