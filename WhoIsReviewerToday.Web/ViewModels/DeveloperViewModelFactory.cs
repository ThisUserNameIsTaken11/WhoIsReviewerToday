using WhoIsReviewerToday.Domain.Models;

namespace WhoIsReviewerToday.Web.ViewModels
{
    internal class DeveloperViewModelFactory : IDeveloperViewModelFactory
    {
        public IDeveloperViewModel Create(Developer developer) => new DeveloperViewModel(developer.UserName, developer.FullName);
    }
}