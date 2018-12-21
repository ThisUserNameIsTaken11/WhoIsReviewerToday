using Telegram.Bot.Types;

namespace WhoIsReviewerToday.Bot.Commands
{
    public class HelpCommand : CommandBase
    {
        public override string Code { get; } = "/help";

        public override void Execute(Message message)
        {
        }
    }
}