namespace WhoIsReviewerToday.Domain.Services
{
    public interface IStartAndStopBotService
    {
        void Start(string websiteUrl);

        void Stop();
    }
}