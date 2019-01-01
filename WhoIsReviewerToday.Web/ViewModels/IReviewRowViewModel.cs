using System;
using System.Collections.Generic;

namespace WhoIsReviewerToday.Web.ViewModels
{
    public interface IReviewRowViewModel
    {
        DateTime DateTime { get; }
        IEnumerable<IDeveloperViewModel> Developers { get; }
    }
}