using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using Quartz;
using WhoIsReviewerToday.Domain.Models;
using WhoIsReviewerToday.Domain.Providers;
using WhoIsReviewerToday.Domain.Repositories;
using WhoIsReviewerToday.Domain.Services;

namespace WhoIsReviewerToday.Infrastructure.Scheduling
{
    internal class MyJob : IJob
    {
        private static readonly Logger _logger = LogManager.GetLogger(nameof(MyJob), typeof(MyJob));
        private readonly ISendMessageService _sendMessageService;
        private readonly IServiceProvider _serviceProvider;

        public MyJob(IServiceProvider serviceProvider, ISendMessageService sendMessageService)
        {
            _serviceProvider = serviceProvider;
            _sendMessageService = sendMessageService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var todayDate = DateTime.Today;
                using (var scope = _serviceProvider.CreateScope())
                {
                    var serviceProvider = scope.ServiceProvider;
                    var reviewByDateProvider = serviceProvider.GetRequiredService<IReviewByDateProvider>();
                    var chatRepository = serviceProvider.GetRequiredService<IChatRepository>();

                    var teams = Enum.GetValues(typeof(Team)).Cast<Team>();
                    var messageStringBuilder = new StringBuilder($"Today is {todayDate:D}, review duties for: {Environment.NewLine}");
                    foreach (var team in teams)
                    {
                        var review = await reviewByDateProvider.GetReview(todayDate, team);
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
                    foreach (var chat in chatRepository.Items)
                        _sendMessageService.SendMessage(chat.TelegramChatId, message);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e);
            }
        }
    }
}