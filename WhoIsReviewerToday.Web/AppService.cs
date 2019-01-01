using Microsoft.Extensions.Configuration;
using WhoIsReviewerToday.Domain.Services;

namespace WhoIsReviewerToday.Web
{
    internal class AppService : IAppService
    {
        private readonly IConfiguration _configuration;
        private readonly ISendReviewDutiesMessageSchedulerService _sendReviewDutiesMessageSchedulerService;
        private readonly IStartAndStopBotService _startAndStopBotService;

        public AppService(
            IConfiguration configuration,
            ISendReviewDutiesMessageSchedulerService sendReviewDutiesMessageSchedulerService,
            IStartAndStopBotService startAndStopBotService)
        {
            _configuration = configuration;
            _sendReviewDutiesMessageSchedulerService = sendReviewDutiesMessageSchedulerService;
            _startAndStopBotService = startAndStopBotService;
        }

        public void Start()
        {
            var websiteUrl = _configuration["Website:Url"];

            _startAndStopBotService.Start(websiteUrl);
            _sendReviewDutiesMessageSchedulerService.Start();
        }

        public void Stop()
        {
            _startAndStopBotService.Stop();
        }
    }
}