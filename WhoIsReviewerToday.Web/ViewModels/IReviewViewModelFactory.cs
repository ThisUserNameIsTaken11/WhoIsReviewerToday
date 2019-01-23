using System;
using System.Collections.Generic;

namespace WhoIsReviewerToday.Web.ViewModels
{
    public interface IReviewViewModelFactory
    {
        IReviewRowViewModel CreateRow(DateTime dateTime, IEnumerable<IDeveloperViewModel> developers);

        IReviewRowListViewModel CreateRowList();

        IReviewViewModel Create(DateTime dateTime, IDeveloperViewModel developer);

        IReviewListViewModel CreateList();
    }
}