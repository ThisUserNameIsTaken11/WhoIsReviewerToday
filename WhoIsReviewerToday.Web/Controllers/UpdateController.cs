using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using WhoIsReviewerToday.Infrastructure.Services;

namespace WhoIsReviewerToday.Web.Controllers
{
    [Route("api/update")]
    [ApiController]
    public class UpdateController : ControllerBase
    {
        private readonly IUpdateService _updateService;

        public UpdateController(IUpdateService updateService)
        {
            _updateService = updateService;
        }

        [HttpPost]
        public IActionResult Post([FromBody] Update update)
        {
            _updateService.Update(update);

            return Ok();
        }
    }
}