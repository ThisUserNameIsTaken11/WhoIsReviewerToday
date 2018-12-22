using System.Linq;
using NLog;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using WhoIsReviewerToday.Infrastructure.Commands;
using WhoIsReviewerToday.Infrastructure.Providers;

namespace WhoIsReviewerToday.Infrastructure.Services
{
    internal class UpdateService : IUpdateService
    {
        private static readonly Logger _logger = LogManager.GetLogger(nameof(UpdateService), typeof(UpdateService));
        private readonly ICommand[] _botCommands;

        public UpdateService(
            IBotCommandProvider botCommandProvider)
        {
            _botCommands = botCommandProvider.GetBotCommands().ToArray();
        }

        public void Update(Update update)
        {
            if (update.Type != UpdateType.Message)
                return;

            var message = update.Message;

            _logger.Info($"Received Message from {message.Chat.Id}, {message.Chat.Username}");

            if (message.Type != MessageType.Text)
                return;

            foreach (var command in _botCommands)
            {
                if (!command.Matches(message.Text))
                    continue;

                command.Execute(message);
            }
        }
    }
}