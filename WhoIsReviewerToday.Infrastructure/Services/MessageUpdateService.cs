using System.Linq;
using NLog;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using WhoIsReviewerToday.Infrastructure.Commands;
using WhoIsReviewerToday.Infrastructure.Providers;

namespace WhoIsReviewerToday.Infrastructure.Services
{
    internal class MessageUpdateService : IMessageUpdateService
    {
        private readonly IChatMembersUpdateService _chatMembersUpdateService;
        private static readonly Logger _logger = LogManager.GetLogger(nameof(MessageUpdateService), typeof(MessageUpdateService));
        private readonly ICommand[] _botCommands;

        public MessageUpdateService(IBotCommandProvider botCommandProvider,
            IChatMembersUpdateService chatMembersUpdateService)
        {
            _chatMembersUpdateService = chatMembersUpdateService;
            _botCommands = botCommandProvider.GetBotCommands().ToArray();
        }

        public void ProcessMessage(Message message)
        {
            _logger.Info($"Received Message from {message.Chat.Id}, {message.Chat.Username}");

            switch (message.Type)
            {
                case MessageType.Text:
                {
                    foreach (var command in _botCommands)
                    {
                        if (!command.Matches(message.Text))
                            continue;

                        command.Execute(message);
                        break;
                    }

                    break;
                }
                case MessageType.ChatMembersAdded:
                    _chatMembersUpdateService.ProcessChatMembersAdded(message);
                    break;
                case MessageType.ChatMemberLeft:
                    _chatMembersUpdateService.ProcessChatMemberLeft(message);
                    break;
                default:
                    return;
            }
        }
    }
}