using Telegram.Bot.Types;

namespace WhoIsReviewerToday.Infrastructure.Commands
{
    public interface ICommand
    {
        void Execute(Message message);

        bool Matches(string commandName);
    }
}