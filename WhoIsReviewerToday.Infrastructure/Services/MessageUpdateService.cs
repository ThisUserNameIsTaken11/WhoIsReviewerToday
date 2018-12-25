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
        private static readonly Logger _logger = LogManager.GetLogger(nameof(MessageUpdateService), typeof(MessageUpdateService));
        private readonly ICommand[] _botCommands;

        public MessageUpdateService(IBotCommandProvider botCommandProvider)
        {
            _botCommands = botCommandProvider.GetBotCommands().ToArray();
        }

        public void ProcessMessage(Message message)
        {
            _logger.Info($"Received Message from {message.Chat.Id}, {message.Chat.Username}");

            if (message.Type != MessageType.Text)
                return;

            foreach (var command in _botCommands)
            {
                if (!command.Matches(message.Text))
                    continue;

                command.Execute(message);
                break;
            }
        }
    }
}