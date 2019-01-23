using System;

namespace WhoIsReviewerToday.Web.ViewModels
{
    public interface IReviewViewModel
    {
        DateTime DateTime { get; }
        IDeveloperViewModel Developer { get; }
    }
}