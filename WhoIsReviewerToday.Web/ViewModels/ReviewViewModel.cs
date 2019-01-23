using System;

namespace WhoIsReviewerToday.Web.ViewModels
{
    public class ReviewViewModel : IReviewViewModel
    {
        public ReviewViewModel(/*DateTime dateTime, IDeveloperViewModel developer*/)
        {
            //DateTime = dateTime;
            //Developer = developer;
        }

        public DateTime DateTime { get; set; }
        public IDeveloperViewModel Developer { get; set;}
    }
}