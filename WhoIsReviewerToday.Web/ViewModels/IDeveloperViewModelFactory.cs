using WhoIsReviewerToday.Domain.Models;

namespace WhoIsReviewerToday.Web.ViewModels
{
    public interface IDeveloperViewModelFactory
    {
        IDeveloperViewModel Create(Developer developer);
    }
}