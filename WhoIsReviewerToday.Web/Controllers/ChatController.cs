using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WhoIsReviewerToday.Domain.Repositories;

namespace WhoIsReviewerToday.Web.Controllers
{
    public class ChatController : Controller
    {
        private readonly IChatRepository _chatRepository;

        public ChatController(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        public IActionResult Index() => View(_chatRepository.Items.ToArray());
    }
}