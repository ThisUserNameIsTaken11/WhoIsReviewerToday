using NLog;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using WhoIsReviewerToday.Bot.Providers;

namespace WhoIsReviewerToday.Bot.Services
{
    internal class WhoIsReviewerTodayService : IWhoIsReviewerTodayService
    {
        private static readonly Logger _logger = LogManager.GetLogger(nameof(WhoIsReviewerTodayService), typeof(WhoIsReviewerTodayService));
        private readonly ICommandProvider _commandProvider;
        private readonly IWhoIsReviewerTodayBot _whoIsReviewerTodayBot;

        public WhoIsReviewerTodayService(
            IWhoIsReviewerTodayBot whoIsReviewerTodayBot,
            ICommandProvider commandProvider)
        {
            _whoIsReviewerTodayBot = whoIsReviewerTodayBot;
            _commandProvider = commandProvider;

            _whoIsReviewerTodayBot.OnUpdate += OnUpdate;
        }

        public void Start()
        {
            _whoIsReviewerTodayBot.StartReceiving();
        }

        public void Stop()
        {
            _whoIsReviewerTodayBot.StopReceiving();
        }

        private void OnUpdate(object sender, UpdateEventArgs e)
        {
            Update(e.Update);
        }

        public void Update(Update update)
        {
            if (update.Type != UpdateType.Message)
                return;

            var message = update.Message;

            _logger.Info($"Received Message from {message.Chat.Id}, {message.Chat.Username}");

            if (message.Type != MessageType.Text)
                return;

            foreach (var command in _commandProvider.Commands)
            {
                if (!command.Matches(message.Text))
                    continue;

                command.Execute(message);
                break;
            }
        }
    }
}