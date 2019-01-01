using System;
using System.Collections.Generic;

namespace WhoIsReviewerToday.Web.ViewModels
{
    public interface IReviewViewModelFactory
    {
        IReviewRowViewModel CreateRow(DateTime dateTime, IEnumerable<IDeveloperViewModel> developers);

        IReviewListViewModel CreateList();
    }
}