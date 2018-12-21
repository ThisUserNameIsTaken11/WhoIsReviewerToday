using Telegram.Bot.Types;

namespace WhoIsReviewerToday.Bot.Commands
{
    public interface ICommand
    {
        string Code { get; }

        void Execute(Message message);

        bool Matches(string commandName);
    }
}