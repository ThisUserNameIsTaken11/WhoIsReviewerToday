using Telegram.Bot.Types;

namespace WhoIsReviewerToday.Infrastructure.Commands
{
    public abstract class CommandBase : ICommand
    {
        protected abstract string Code { get; }

        public abstract bool Matches(string commandName);

        public abstract void Execute(Message message);
    }
}