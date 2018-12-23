using Telegram.Bot.Types;
using WhoIsReviewerToday.Bot;

namespace WhoIsReviewerToday.Infrastructure.Commands
{
    public class HelpCommand : SingleWordCommandBase
    {
        private readonly IWhoIsReviewerTodayService _whoIsReviewerTodayService;

        public HelpCommand(IWhoIsReviewerTodayService whoIsReviewerTodayService)
        {
            _whoIsReviewerTodayService = whoIsReviewerTodayService;
        }

        protected override string Code { get; } = "/help";

        public override void Execute(Message message)
        {
            var chatId = new ChatId(message.Chat.Id);
            _whoIsReviewerTodayService.SendSimpleMessage(chatId, "this is help command");
        }
    }
}