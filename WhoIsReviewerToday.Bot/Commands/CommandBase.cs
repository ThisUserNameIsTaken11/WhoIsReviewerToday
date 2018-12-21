using Telegram.Bot.Types;

namespace WhoIsReviewerToday.Bot.Commands
{
    public abstract class CommandBase : ICommand
    {
        public abstract string Code { get; }

        public abstract void Execute(Message message);

        public bool Matches(string commandName) => commandName.Contains(Code);
    }
}