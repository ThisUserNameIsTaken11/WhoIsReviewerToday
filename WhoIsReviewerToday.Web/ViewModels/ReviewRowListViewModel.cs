using System;
using System.Collections.Generic;
using System.Linq;
using WhoIsReviewerToday.Domain.Models;
using WhoIsReviewerToday.Domain.Repositories;

namespace WhoIsReviewerToday.Web.ViewModels
{
    internal class ReviewRowListViewModel : IReviewRowListViewModel
    {
        public ReviewRowListViewModel(
            IReviewRepository reviewRepository,
            IReviewViewModelFactory reviewViewModelFactory,
            IDeveloperViewModelFactory developerViewModelFactory)
        {
            var reviewRowViewModels = new List<IReviewRowViewModel>();
            foreach (var groupedReview in reviewRepository.Items
                .OrderBy(r => r.DateTime)
                .GroupBy(r => r.DateTime.Date))
            {
                var developerViewModels = groupedReview.Select(review => review.Developer)
                    .OrderBy(developer => developer.Team)
                    .Select(developerViewModelFactory.Create);

                reviewRowViewModels.Add(reviewViewModelFactory.CreateRow(groupedReview.Key, developerViewModels.ToArray()));
            }

            Headers = new[] { "Date" }.Concat(Enum.GetValues(typeof(Team)).Cast<Team>().Select(team => team.ToString()));
            Rows = reviewRowViewModels;
        }

        public IEnumerable<string> Headers { get; }

        public IEnumerable<IReviewRowViewModel> Rows { get; }
    }
}