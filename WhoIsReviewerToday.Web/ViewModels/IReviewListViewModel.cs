using System.Collections.Generic;

namespace WhoIsReviewerToday.Web.ViewModels
{
    public interface IReviewListViewModel
    {
        IEnumerable<IReviewViewModel> Items { get; }
    }
}