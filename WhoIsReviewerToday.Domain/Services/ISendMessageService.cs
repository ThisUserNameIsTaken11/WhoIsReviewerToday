namespace WhoIsReviewerToday.Domain.Services
{
    public interface ISendMessageService
    {
        void SendMessage(long telegramChatId, string text);

        void SendMessage(string username, string text);
    }
}