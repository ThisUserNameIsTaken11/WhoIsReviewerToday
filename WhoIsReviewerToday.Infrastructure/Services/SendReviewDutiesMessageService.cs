using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using WhoIsReviewerToday.Domain.Models;
using WhoIsReviewerToday.Domain.Repositories;
using WhoIsReviewerToday.Domain.Services;
using WhoIsReviewerToday.Infrastructure.Providers;

namespace WhoIsReviewerToday.Infrastructure.Services
{
    internal class SendReviewDutiesMessageService : ISendReviewDutiesMessageService
    {
        private static readonly Logger _logger = LogManager.GetLogger(nameof(SendReviewDutiesMessageService), typeof(SendReviewDutiesMessageService));
        private readonly IChatRepository _chatRepository;
        private readonly IReviewByDateProvider _reviewByDateProvider;
        private readonly ISendMessageService _sendMessageService;

        public SendReviewDutiesMessageService(
            ISendMessageService sendMessageService,
            IReviewByDateProvider reviewByDateProvider,
            IChatRepository chatRepository)
        {
            _sendMessageService = sendMessageService;
            _reviewByDateProvider = reviewByDateProvider;
            _chatRepository = chatRepository;
        }

        public async Task SendMessage(DateTime dateTime)
        {
            try
            {
                var teams = Enum.GetValues(typeof(Team)).Cast<Team>();

                var messageStringBuilder = new StringBuilder($"{dateTime:D}, review duties for: {Environment.NewLine}");
                foreach (var team in teams)
                {
                    var review = await _reviewByDateProvider.GetReview(dateTime, team);
                    if (review == null)
                    {
                        var failMessageForTeam = $"Could not get review duty for {team} team";
                        _logger.Info(failMessageForTeam);
                        messageStringBuilder.AppendLine(failMessageForTeam);
                        continue;
                    }

                    messageStringBuilder.AppendLine($"{team} team - {review.Developer.UserName}");
                }

                var message = messageStringBuilder.ToString();
                foreach (var chat in _chatRepository.Items.ToArray())
                    if (!await _sendMessageService.TrySendMessageAsync(chat.TelegramChatId, message))
                        _chatRepository.TryRemoveChatAndSave(chat);
            }
            catch (Exception e)
            {
                _logger.Error(e);
            }
        }
    }
}