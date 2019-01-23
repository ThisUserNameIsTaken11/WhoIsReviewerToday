using System.Collections.Generic;

namespace WhoIsReviewerToday.Web.ViewModels
{
    public interface IReviewRowListViewModel
    {
        IEnumerable<string> Headers { get; }
        IEnumerable<IReviewRowViewModel> Rows { get; }
    }
}