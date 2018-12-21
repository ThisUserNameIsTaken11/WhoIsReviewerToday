using Telegram.Bot.Types;

namespace WhoIsReviewerToday.Bot.Commands
{
    public class StartCommand : CommandBase
    {
        public override string Code { get; } = "/start";

        public override void Execute(Message message)
        {
        }
    }
}