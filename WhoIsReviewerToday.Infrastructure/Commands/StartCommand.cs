using Telegram.Bot.Types;
using WhoIsReviewerToday.Bot;

namespace WhoIsReviewerToday.Infrastructure.Commands
{
    public class StartCommand : SingleWordCommandBase
    {
        private readonly IWhoIsReviewerTodayService _whoIsReviewerTodayService;

        public StartCommand(IWhoIsReviewerTodayService whoIsReviewerTodayService)
        {
            _whoIsReviewerTodayService = whoIsReviewerTodayService;
        }

        protected override string Code { get; } = "/start";

        public override void Execute(Message message)
        {
        }
    }
}