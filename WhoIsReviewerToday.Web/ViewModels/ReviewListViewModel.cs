using System.Collections.Generic;
using System.Linq;
using WhoIsReviewerToday.Domain.Repositories;

namespace WhoIsReviewerToday.Web.ViewModels
{
    internal class ReviewListViewModel : IReviewListViewModel
    {
        public ReviewListViewModel(
            IReviewRepository reviewRepository,
            IReviewViewModelFactory reviewViewModelFactory,
            IDeveloperViewModelFactory developerViewModelFactory)
        {
            Items = reviewRepository.Items
                .OrderBy(review => review.DateTime)
                .Select(review => reviewViewModelFactory.Create(review.DateTime, developerViewModelFactory.Create(review.Developer)))
                .ToArray();
        }

        public IEnumerable<IReviewViewModel> Items { get; }
    }
}