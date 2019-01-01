using System.Collections.Generic;

namespace WhoIsReviewerToday.Web.ViewModels
{
    public interface IReviewListViewModel
    {
        IEnumerable<string> Headers { get; }
        IEnumerable<IReviewRowViewModel> Rows { get; }
    }
}