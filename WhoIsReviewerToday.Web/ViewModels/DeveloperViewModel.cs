namespace WhoIsReviewerToday.Web.ViewModels
{
    internal class DeveloperViewModel : IDeveloperViewModel
    {
        public DeveloperViewModel(string userName, string fullName)
        {
            UserName = userName;
            FullName = fullName;
        }

        public string UserName { get; }
        public string FullName { get; }
    }
}