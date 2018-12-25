using NLog;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace WhoIsReviewerToday.Infrastructure.Services
{
    internal class UpdateService : IUpdateService
    {
        private static readonly Logger _logger = LogManager.GetLogger(nameof(UpdateService), typeof(UpdateService));
        private readonly IMessageUpdateService _messageUpdateService;

        public UpdateService(
            IMessageUpdateService messageUpdateService)
        {
            _messageUpdateService = messageUpdateService;
        }

        public void Update(Update update)
        {
            _logger.Info($"Received Update ({update.Type})");

            switch (update.Type)
            {
                case UpdateType.Message:
                    _messageUpdateService.ProcessMessage(update.Message);
                    break;
                default:
                    return;
            }
        }
    }
}