using Microsoft.AspNetCore.Mvc;
using WhoIsReviewerToday.Domain.Repositories;

namespace WhoIsReviewerToday.Web.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IReviewRepository _reviewRepository;

        public ReviewController(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public IActionResult Index() => View();
    }
}