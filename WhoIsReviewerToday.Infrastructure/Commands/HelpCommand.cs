using Telegram.Bot.Types;
using WhoIsReviewerToday.Domain.Services;

namespace WhoIsReviewerToday.Infrastructure.Commands
{
    public class HelpCommand : SingleWordCommandBase
    {
        private readonly ISendMessageService _sendMessageService;

        public HelpCommand(ISendMessageService sendMessageService)
        {
            _sendMessageService = sendMessageService;
        }

        protected override string Code { get; } = "/help";

        public override void Execute(Message message)
        {
            _sendMessageService.TrySendMessageAsync(message.Chat.Id, "I can't help you! Sorry!");
        }
    }
}