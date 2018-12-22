using Telegram.Bot.Types;

namespace WhoIsReviewerToday.Infrastructure.Commands
{
    public class HelpCommand : SingleWordCommandBase
    {
        protected override string Code { get; } = "/help";

        public override void Execute(Message message)
        {
        }
    }
}