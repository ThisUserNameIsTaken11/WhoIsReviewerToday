using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using WhoIsReviewerToday.Bot.Services;

namespace WhoIsReviewerToday.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateController : ControllerBase
    {
        private readonly IWhoIsReviewerTodayService _whoIsReviewerTodayService;

        public UpdateController(IWhoIsReviewerTodayService whoIsReviewerTodayService)
        {
            _whoIsReviewerTodayService = whoIsReviewerTodayService;
        }

        [HttpPost]
        public IActionResult Post([FromBody] Update update)
        {
            _whoIsReviewerTodayService.Update(update);

            return Ok();
        }
    }
}