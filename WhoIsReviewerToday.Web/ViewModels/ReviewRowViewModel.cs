using System;
using System.Collections.Generic;

namespace WhoIsReviewerToday.Web.ViewModels
{
    internal class ReviewRowViewModel : IReviewRowViewModel
    {
        public ReviewRowViewModel(DateTime dateTime, IEnumerable<IDeveloperViewModel> developers)
        {
            DateTime = dateTime;
            Developers = developers;
        }

        public DateTime DateTime { get; }

        public IEnumerable<IDeveloperViewModel> Developers { get; }
    }
}