using Microsoft.AspNetCore.Mvc;
using WhoIsReviewerToday.Web.ViewModels;

namespace WhoIsReviewerToday.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IReviewViewModelFactory _reviewViewModelFactory;

        public HomeController(IReviewViewModelFactory reviewViewModelFactory)
        {
            _reviewViewModelFactory = reviewViewModelFactory;
        }

        public IActionResult Index() => View(_reviewViewModelFactory.CreateList());
    }
}