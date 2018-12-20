using Microsoft.AspNetCore.Mvc;
using WhoIsReviewerToday.Domain.Repositories;

namespace WhoIsReviewerToday.Web.Controllers
{
    public class DeveloperController : Controller
    {
        private readonly IDeveloperRepository _developerRepository;

        public DeveloperController(IDeveloperRepository developerRepository)
        {
            _developerRepository = developerRepository;
        }

        public IActionResult Index() => View(_developerRepository.Items);
    }
}